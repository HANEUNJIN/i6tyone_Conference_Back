using eGhis_WebService_Core.Filter;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Extensions;
using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Middleware;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Config;
using eGhis_WebService_Core.Repositories;
using eGhis_WebService_Core.Swagger;
using FluentValidation;
using i6tyone_Conference.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
// using Elastic.Apm.NetCoreAll; // ← Elastic APM 쓰면 주석 해제

namespace eGhis_WebService_Core
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                var builder = CreateBuilder(args);

                builder.Host.UseSerilog((ctx, services, cfg) =>
                {
                    var logDir = Path.Combine(AppContext.BaseDirectory, "_log");
                    Directory.CreateDirectory(logDir);

                    cfg.ReadFrom.Configuration(ctx.Configuration)
                       .ReadFrom.Services(services)
                       .Enrich.FromLogContext()
                       .WriteTo.File(
                           path: Path.Combine(logDir, "Admin_Error_.log"),
                           rollingInterval: RollingInterval.Day,
                           retainedFileCountLimit: 30,
                           shared: true);
                });

                ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

                var app = builder.Build();
                ConfigureMiddleware(app);

                app.MapControllers();
                app.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        // =====================================================================
        // ✅ 환경 설정 + Configuration 수동 구성
        // =====================================================================
        private static WebApplicationBuilder CreateBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = Directory.GetCurrentDirectory(),
                EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
            });

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder;
        }

        // =====================================================================
        // ✅ ConfigureServices 등록 (DB_VENDOR 기반 단일 커넥션 선택)
        // =====================================================================
        private static void ConfigureServices(IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<FluentValidationFilter>();

            // Controllers + 전역 필터 (한 번만 등록)
            services.AddControllers(options =>
            {
                options.Filters.Add<HttpMethodFilter>();
                options.Filters.Add<FluentValidationFilter>(); 
            });

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = ctx =>
                {
                    var errors = ctx.ModelState
                        .Where(kv => kv.Value?.Errors.Count > 0)
                        .Select(kv => new {
                            Field = kv.Key,
                            Errors = kv.Value!.Errors.Select(e => e.ErrorMessage)
                        });
                    return new BadRequestObjectResult(ResponseBaseModel.Fail("400", "유효성 검사 실패", errors));
                };
            });

            // ── 연결 문자열(복호화 포함)
            var egisConn = config.GetConnectionString("Connection");
            if (string.IsNullOrWhiteSpace(egisConn))
                throw new InvalidOperationException("ConnectionStrings: Connection 누락");

            // 프로필(launchSettings)에서 DB_VENDOR = Eghis | Nix
            var pick = (Environment.GetEnvironmentVariable("DB_VENDOR") ?? "Eghis").ToLowerInvariant();

            // 실제 사용 커넥션은 하나(ClinicConn)로만 흘려보낸다
            services.Configure<ConnectionString>(o =>
            {
                o.ClinicConn = egisConn;
            });

            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));

            // ── CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            // ── NSwag (Swagger) + 보안 + ReDoc/Stoplight 대응
            services.AddOpenApiDocument(cfg =>
            {
                cfg.Title = "i6tyone_Conference API";
                cfg.Version = "v1";
                cfg.DocumentName = "v1";
                cfg.Description = "i6tyone_Conference의 OpenAPI 문서입니다.";

                // Bearer
                cfg.AddSecurity("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Enter 'Bearer {token}'"
                });

                cfg.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

                // 커스텀 Operation Processor들
                cfg.OperationProcessors.Add(new NSwagResponseFilter());
                cfg.OperationProcessors.Add(new RemoveNullableBinaryOperationProcessor());

                cfg.PostProcess = doc =>
                {
                    doc.Info.Version = "v1";
                    doc.Info.Title = "i6tyone_Conference API";
                    doc.Info.Description =
                                           "ℹ️ API 상세설명\n\r" +
                                           "https://www.notion.so/28d3d9f986ea805b8034d3a2989a086d?v=2913d9f986ea80cba6b0000c30813ab7&source=copy_link\n\r" +
                                           "ℹ️ 컨퍼런스 등록부스 개발 관련 소통\n\r" +
                                           "https://www.notion.so/28c3d9f986ea815cb2c8d1c03c80ac9a?source=copy_link";

                    doc.Info.Contact = new OpenApiContact
                    {
                        //Name = "한은진",
                        //Url = "http://172.12.2.204:5001/ReDoc"
                    };
                };
            });

            //var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = jwtSettings.Issuer,
            //            ValidAudience = jwtSettings.Audience,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            //            NameClaimType = "sub",
            //            RoleClaimType = "role"
            //        };
            //    });


            services.AddAuthorization();

            // ── Repository/Service/AutoMapper
            services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<ISqlRepository, SqlRepository>();
            services.AddScoped<JwtUtil>();
            services.AddScoped<QRCodeUtil>();

            // DAO/Service 일괄 등록
            services.AddDaos(typeof(Program).Assembly);
            services.AddServices(typeof(Program).Assembly);

            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperUtil>());
        }

        // =====================================================================
        // ✅ 미들웨어 등록 (Exception → StaticFiles → Routing → CORS → Auth → Swagger)
        // =====================================================================
        private static void ConfigureMiddleware(WebApplication app)
        {
            // 1) 전역 예외 표준화 (가장 먼저)
            app.UseMiddleware<ExceptionMiddleware>();

            // 2) 정적 파일
            app.UseStaticFiles();

            // 3) 개발자 페이지(개발 환경에서만)
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 4) 라우팅
            app.UseRouting();

            // 5) CORS
            app.UseCors("AllowAll");

            // 6) 요청 로깅(Serilog)
            app.UseSerilogRequestLogging();

            // 7) 인증/권한
            app.UseAuthentication();
            app.UseAuthorization();

            // 8) 커스텀 토큰 미들웨어
            app.UseMiddleware<TokenValidationMiddleware>();

            // 9) OpenAPI(JSON)
            app.UseOpenApi();

            // 10) Swagger UI
            app.UseSwaggerUi(settings =>
            {
                settings.Path = "/swagger";
                settings.DocumentPath = "/swagger/v1/swagger.json";
            });

            // 11) ReDoc
            app.UseReDoc(settings =>
            {
                settings.Path = "/redoc";
                settings.DocumentPath = "/swagger/v1/swagger.json";
                settings.DocumentTitle = "i6tyone_Conference ReDoc";
            });

            // 12) Stoplight Elements
            app.UseStoplight("/stoplight", "/swagger/v1/swagger.json");
        }
    }
}
