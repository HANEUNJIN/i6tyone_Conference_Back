// Infrastructure/Utils/LoggerUtil.cs
using Serilog;
using Serilog.Events;

namespace eGhis_WebService_Core.Infrastructure.Utils
{
    public static class LoggerUtil
    {
        private static int _bootstrapped = 0;

        public static void Initialize(WebApplicationBuilder builder)
        {
            if (Interlocked.Exchange(ref _bootstrapped, 1) == 0)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss.fff}][{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateBootstrapLogger();
            }

            builder.Host.UseSerilog((ctx, sp, cfg) =>
            {
                var env = ctx.HostingEnvironment;
                var conf = ctx.Configuration;

                var logDir = Path.Combine(AppContext.BaseDirectory, "_log");
                Directory.CreateDirectory(logDir);

                cfg.ReadFrom.Configuration(conf)  
                   .ReadFrom.Services(sp)
                   .Enrich.FromLogContext()
                   .Enrich.WithMachineName()
                   .Enrich.WithThreadId()
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

                if (env.IsDevelopment())
                {
                    cfg.WriteTo.Console(
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level:u3}][{ThreadId}][{SourceContext}] {Message:lj}{NewLine}{Exception}");
                }

                cfg.WriteTo.File(
                    path: Path.Combine(logDir, "Admin_Error_.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    shared: true,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level:u3}][{ThreadId}][{SourceContext}] {Message:lj}{NewLine}{Exception}");
            });
        }

        public static void Shutdown() => Log.CloseAndFlush();

        private static void Write(LogEventLevel level, string code, string title, string message, Exception? ex = null)
        {
            var log = Log.ForContext("code", code)
                         .ForContext("title", title)
                         .ForContext("contents", message);

            if (ex is not null) log.Write(level, ex, "{Title} - {Contents}", title, message);
            else log.Write(level, "{Title} - {Contents}", title, message);
        }

        public static void Debug(string code, string title, string message) =>
            Write(LogEventLevel.Debug, code, title, message);

        public static void Info(string code, string title, string message) =>
            Write(LogEventLevel.Information, code, title, message);

        public static void Warn(string code, string title, string message) =>
            Write(LogEventLevel.Warning, code, title, message);

        public static void Error(string code, string title, string message) =>
            Write(LogEventLevel.Error, code, title, message);

        public static void Error(string code, string title, string message, Exception ex) =>
            Write(LogEventLevel.Error, code, title, message, ex);

        public static void Fatal(string code, string title, string message) =>
            Write(LogEventLevel.Fatal, code, title, message);

        public static void Fatal(string code, string title, string message, Exception ex) =>
            Write(LogEventLevel.Fatal, code, title, message, ex);
    }
}
