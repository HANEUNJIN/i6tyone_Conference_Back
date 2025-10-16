
# i6tyone_Conference API

`i6tyone_Conference`는 아이자야씩스티원 컨퍼런스 등록 관리용 백엔드 API 시스템입니다.  
.NET 8 기반으로 구성되었으며, 환경별 설정, 미들웨어, Swagger 문서화(NSwag), 예외 처리 등을 포함하고 있습니다.

---

## 🏗️ 프로젝트 구성

```bash
📦 i6tyone_Conference
├─ Controllers/                # API 컨트롤러
├─ DbAccess/
│  └─ DbContexts/             # EF Core DbContext 정의
├─ Filter/
│  └─ HttpMethodFilter.cs     # 허용되지 않은 HTTP 메서드 차단
│  └─ NSwagResponseFilter.cs  # Swagger 공통 응답 필터
├─ Infrastructure/
│  └─ Utils/                  # 공통 유틸리티 (암호화, Enum 유틸 등)
├─ Middleware/
│  └─ ExceptionMiddleware.cs  # 전역 예외 처리 미들웨어
├─ Models/
│  └─ Common/                 # 공통 응답 모델
│  └─ Config/                 # 환경별 ConnectionString 모델
├─ Define/
│  └─ ErrorStatusCode.cs      # 공통 오류 코드 Enum
├─ Program.cs                 # 진입점 및 설정 구성
└─ appsettings.{env}.json     # 환경별 구성 파일
```

---

## 🛠️ 실행 전 준비 사항

### 1. 필수 NuGet 패키지

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.AspNetCore.Http.Abstractions
dotnet add package NSwag.AspNetCore
dotnet add package Swashbuckle.AspNetCore
dotnet add package Newtonsoft.Json
dotnet add package FluentValidation.AspNetCore
```

### 2. 환경설정

- `appsettings.json` 및 `appsettings.{Environment}.json` 에 다음 항목 필요:

```json
{
  "ConnectionStrings": {
    "ConnectionString": "{Encrypted or Plain Connection String}",
    "ApiConnectionString": "{Encrypted or Plain API DB Connection}"
  }
}
```

---

## 🚀 실행 방법

```bash
# 개발 환경 지정
set ASPNETCORE_ENVIRONMENT=Development

# 실행
dotnet run --project i6tyone_Conference
```

---

## 📚 API 문서화 (NSwag + Swagger UI + ReDoc + Stoplight)

| 문서 타입   | 경로                       |
|------------|----------------------------|
| Swagger UI | `http://localhost:{port}/swagger` |
| ReDoc      | `http://localhost:{port}/redoc`   |
| Stoplight  | `http://localhost:{port}/stoplight`               |
| JSON Doc   | `http://localhost:{port}/swagger/v1/swagger.json` |

> 📌 `NSwagResponseFilter`를 통해 모든 API에 200/400/401/403/500 응답 스키마 자동 등록됨

---

## ✅ 미들웨어 및 필터 구성

- `HttpMethodFilter`: 허용되지 않은 HTTP 메서드 차단 (GET, POST, PUT, PATCH, DELETE만 허용)
- `ExceptionMiddleware`: BusinessException 및 일반 예외 처리
- `EnumUtil`: `Display`, `Description` 속성 기반 응답 메시지 처리

---

## 👤 담당자

- 한은진
- 문의: `010-3532-8236`