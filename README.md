# eGhis WebService Core

병원 **Core API 시스템**  
ASP.NET Core 8 기반, Dapper 기반 SQL 아키텍처 + NSwag/Stoplight 문서화 적용

---

## 📁 프로젝트 폴더 구조

```
📆 eGhis_WebService_Core/
│
├── 🗭 eGhis_WebService_Core/           # 메인 API C# 프로젝트 (ASP.NET Core Web API)
│   ├──  Controllers/                  # API 컨트롤러
│   ├──  Define/                       # 공통 상수, Enum 정의
│   ├──  DbAccess/
│   │   └──  Dao/                      # Dapper 기반 DAO (SQL 실행부)
│   ├──  Filter/                       # 필터 (예외 처리, Swagger 응답 등)
│   ├──  Infrastructure/
│   │   ├──  Utils/                    # 공통 유틸 (암복호화, JWT 등)
│   │   ├──  Db/                       # DB 커넥션 팩토리
│   │   └──  Extensions/               # 확장 메서드 모음
│   ├──  Middleware/                   # 전역 미들웨어 (예외 처리 등)
│   ├──  Models/
│   │   ├──  Config/                   # appsettings 바인딩 클래스
│   │   ├──  Db/                       # Dapper Record 매핑 모델
│   │   └──  Dto/                      # Request/Response DTO
│   ├──  Repositories/                 # ISqlRepository, SqlRepository (DAO 게이트)
│   ├──  Service/                      # 도메인 서비스 (Auth 등)
│   ├──  Swagger/                      # Swagger/NSwag 관련 커스텀 프로세서
│   ├── 📄 Program.cs                  # 앱 구성 및 서비스 등록
│   └── 📄 appsettings.{env}.json      # 환경별 설정 파일
│
└── 📄 README.md                       # 프로젝트 구조 및 설명 파일
```

---

## ⚙️ 사용 프레임워크 및 주요 NuGet

- **Target Framework**: `net8.0`

### 주요 패키지

- `Dapper` – 경량 SQL Mapper
- `Npgsql` – PostgreSQL 연동
- `Serilog` – 파일/콘솔 로깅
- `NSwag.AspNetCore` – OpenAPI/Swagger 문서화
- `Newtonsoft.Json` – JSON 직렬화/역직렬화

---

## 🛠️ 빌드 방법

Linux 환경에서 실행 가능한 형태로 **Release 모드로 퍼블리시** 하려면:

```bash
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
```

> 📁 `./publish` 폴더에 생성된 파일을 서버에 업로드합니다.

---

## 🚀 배포 방법

### 1️⃣ 서비스 중지

```bash
sudo systemctl stop eghis_core_api
```

### 2️⃣ 최신 파일 덮어쓰기

`./publish` 폴더 내의 파일을 서버 디렉토리에 업로드해서 기존 파일 대체

### 3️⃣ 서비스 실행

```bash
# 개발 환경에서 직접 실행
ASPNETCORE_ENVIRONMENT=Development dotnet eGhis_WebService_Core.dll

# 운영 환경은 systemd 로 실행
sudo systemctl start eghis_core_api
```

---

## 📄 로그 확인

```bash
# 실시간 로그 확인
journalctl -fu eghis_core_api

# 최근 로그 100줄 확인
journalctl -u eghis_core_api -n 100
```

---

## 🧭 전체 구조 흐름도

```
┌────────────┐
│  Client    │  (ex. 관리자 Web)
└────┬───────┘
     │ HTTP 요청 (Token 포함)
     ▼
┌───────────────┐
│  Middleware   │  (토큰 검증, 예외 처리, 로깅)
└────┬──────────┘
     ▼
┌──────────────┐
│  Controller  │  (ex. AuthController 등)
└────┬─────────┘
     ▼
┌──────────────┐
│   Service    │  (도메인 비즈니스 로직 처리)
└────┬─────────┘
     ▼
┌──────────────┐
│ Repository   │  (DAO 게이트, 트랜잭션 관리)
└────┬─────────┘
     ▼
┌──────────────┐
│     DAO      │  (Dapper SQL 실행)
└────┬─────────┘
     ▼
┌──────────────┐
│   Database   │  (PostgreSQL)
└──────────────┘

* 공통 기능은 아래처럼 보조 역할
├── Infrastructure (암복호화, JWT, 커넥션 팩토리 등)
├── Filter (전역 예외, Swagger 공통 응답 등)
├── Models (DTO, Record, Config)
```

---

## 📚 문서

- [Swagger 문서 보기](http://localhost:5288/swagger)  
- [ReDoc 문서 보기](http://localhost:5288/redoc)  
- [Stoplight 문서 보기](http://localhost:5288/stoplight)  

---
