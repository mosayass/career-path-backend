# CareerPath Backend

A comprehensive **.NET 10 microservices-based backend** for the CareerPath platform that helps users discover and explore career paths based on their skills and assessments. The backend integrates with a **Python AI inference microservice** for intelligent career predictions and recommendations.

**Repository:** https://github.com/mosayass/career-path-backend

---

## Table of Contents

- [Project Overview](#project-overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Modules](#modules)
- [Setup Instructions](#setup-instructions)
  - [Prerequisites](#prerequisites)
  - [Environment Configuration](#environment-configuration)
  - [Running with Docker Compose](#running-with-docker-compose)
  - [Running Locally (Development)](#running-locally-development)
- [AI Inference Integration](#ai-inference-integration)
- [API Documentation](#api-documentation)
- [Database Migrations](#database-migrations)
- [Contributing](#contributing)

---

## Project Overview

CareerPath Backend is a graduation project designed to provide a robust platform for career pathway discovery. The system combines multiple modules to handle user authentication, career data, assessments, community features, and user profiles.

**Key Features:**
- 🔐 **Identity & Authentication**: JWT-based authentication with ASP.NET Identity
- 📊 **Assessment Engine**: Submit assessments and receive AI-powered career predictions
- 💼 **Career Data**: Browse and explore career information and requirements
- 👥 **Community Features**: Posts, discussions, and user interactions
- 👤 **User Profiles**: Personalized user profiles and career tracking
- 🤖 **AI Integration**: Python-based FastAPI inference service for career recommendations
- ☁️ **Cloud Storage**: Azure Blob Storage support (Azurite for local development)

---

## Architecture

### System Design

```
┌─────────────────────────────────────────────────────────────────┐
│                      Frontend Application                        │
│                    (React @ localhost:3003)                      │
└────────────────────────┬────────────────────────────────────────┘
                         │ HTTP/REST
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│                  ASP.NET Core Backend                            │
│              (localhost:8080 / port 8080)                        │
├─────────────────────────────────────────────────────────────────┤
│  Modules:                                                        │
│  • Identity Module (Auth, JWT, User Management)                 │
│  • Assessment Module (Assessments, AI Integration)              │
│  • Career Module (Career Data, Requirements)                    │
│  • Community Module (Posts, Discussions, Storage)               │
│  • Profiles Module (User Profiles, Career Tracking)             │
├─────────────────────────────────────────────────────────────────┤
│  Infrastructure:                                                 │
│  • PostgreSQL Database (localhost:5433)                         │
│  • Azure Blob Storage / Azurite (localhost:10000)              │
└────────────┬──────────────────────────────────────────────┬─────┘
             │ HTTP                                         │ HTTP
             ▼                                              ▼
    ┌──────────────────┐                         ┌──────────────────────┐
    │  PostgreSQL DB   │                         │  Python FastAPI      │
    │  (port 5432)     │                         │  AI Service          │
    └──────────────────┘                         │  (localhost:8000)    │
                                                 │                      │
                                                 │ Career Prediction    │
                                                 │ ML Model             │
                                                 └──────────────────────┘
```

### Modular Architecture

The backend follows a **Vertical Slice Architecture** with clean separation of concerns:

```
Modules/
├── Assessment/
│   ├── Api/              # API Controllers & Endpoints
│   ├── Core/             # Business Logic, DTOs, Entities
│   └── Infrastructure/   # Database, External Services
├── Careers/
│   ├── Api/
│   ├── Core/
│   └── Infrastructure/
├── Community/
│   ├── Api/
│   ├── Core/
│   └── Infrastructure/
├── Identity/
│   ├── Api/
│   ├── Core/
│   └── Infrastructure/
├── Profiles/
│   ├── Api/
│   ├── Core/
│   └── Infrastructure/
└── ...

Shared/
├── CareerPath.Shared/                   # Common models & utilities
├── CareerPath.Shared.Api/               # Shared API infrastructure
├── CareerPath.Shared.Infrastructure/    # Shared persistence logic
└── CareerPath.Shared.IntegrationEvents/ # Event-driven messaging

Host/
└── CareerPath.Host/                     # Main application host & startup
```

---

## Tech Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| **Framework** | ASP.NET Core / .NET | 10.0 |
| **Database** | PostgreSQL | 16 Alpine |
| **Storage** | Azure Blob Storage (Azurite) | Latest |
| **ORM** | Entity Framework Core | Latest |
| **Authentication** | JWT + ASP.NET Identity | - |
| **API Documentation** | Swagger/OpenAPI | - |
| **Email** | Mailtrap SMTP | - |
| **Containerization** | Docker & Docker Compose | Latest |
| **AI Service** | Python FastAPI | 3.9+ |
| **Machine Learning** | Python scikit-learn, pandas, numpy | Latest |

---

## Modules

### 1. **Identity Module** (`Modules/Identity`)
Handles user authentication, registration, JWT token generation, and identity management.

**Key Components:**
- `AuthController` - Login, Register, Token Refresh endpoints
- `RegisterCommandHandler` - Business logic for user registration
- `MailtrapEmailService` - Email verification and password reset
- `IdentityDbContext` - ASP.NET Identity database context

**Key Features:**
- JWT-based authentication
- Email verification
- Password reset functionality
- Role-based authorization

---

### 2. **Assessment Module** (`Modules/Assessment`)
Manages user assessments and integrates with the Python AI inference service for career predictions.

**Key Components:**
- `AssessmentsController` - Assessment submission & retrieval endpoints
- `FastApiAiModelClient` - HTTP client for Python AI service communication
- `SubmitAssessmentCommandHandler` - Orchestrates assessment processing and AI prediction
- `AssessmentResultConfiguration` - EF Core configuration for results persistence

**Key Features:**
- Submit assessments with user responses
- Call Python FastAPI service for career predictions
- Store assessment results and AI predictions
- Retrieve historical assessments

**AI Integration:**
- Endpoint: `/predict/top-matches` (FastAPI service)
- Input: `AssessmentSubmissionPayload` (user responses)
- Output: `AiPredictionResponse` (top career matches with scores)

---

### 3. **Career Module** (`Modules/Careers`)
Provides career data, requirements, and skill mappings.

**Key Components:**
- `CareersController` - Career browsing & search endpoints
- Career entity with requirements and skill associations
- Career repository for data access

**Key Features:**
- Browse available careers
- View career requirements
- Search careers by keyword/skill
- Career skill mapping and profiling

---

### 4. **Community Module** (`Modules/Community`)
Enables user interactions, posts, discussions, and file uploads.

**Key Components:**
- `PostsController` - Create, read, update, delete posts
- `AzureBlobStorageService` - Cloud file storage management
- `CommunityDataSeeder` - Initial community data setup
- Community entities (Posts, Comments, etc.)

**Key Features:**
- Create and manage community posts
- Upload files to Azure Blob Storage (or Azurite locally)
- CORS support for frontend connections
- Community data seeding

---

### 5. **Profiles Module** (`Modules/Profiles`)
Manages user profile information and career pathway tracking.

**Key Components:**
- `ProfilesController` - Profile CRUD operations
- User profile entity and persistence
- Career pathway tracking

**Key Features:**
- View and update user profiles
- Track career pathway history
- Profile picture/avatar management
- Career preference storage

---

## Setup Instructions

### Prerequisites

Before you begin, ensure you have the following installed:

- **Docker & Docker Compose** (version 20.10+)
- **.NET 10 SDK** (for local development without Docker)
- **PostgreSQL** (optional, included in Docker Compose)
- **Git** (to clone the repository)
- **Python 3.9+** (if running the AI service locally)

### Environment Configuration

The project uses `appsettings.json` for configuration. Key settings include:

**appsettings.json** - Default local development settings:
```json
{
  "JwtOptions": {
    "SecretKey": "QJEYm=`a3GvIp$<[nGj262qX(E^kB,@8#X",
    "Issuer": "CareerPathApi",
    "Audience": "CareerPathClients",
    "ExpirationInMinutes": 60
  },
  "EmailSettings": {
    "Host": "sandbox.smtp.mailtrap.io",
    "Port": 2525,
    "Username": "YOUR_MAILTRAP_USERNAME",
    "Password": "YOUR_MAILTRAP_PASSWORD"
  },
  "AiModelService": {
    "BaseUrl": "http://localhost:8000/"
  },
  "Storage": {
    "IsLocalContainer": true,
    "ConnectionString": "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;..."
  }
}
```

**Environment Variables** (used in Docker Compose):
- `ASPNETCORE_ENVIRONMENT`: Development or Production
- `ConnectionStrings__DefaultConnection`: PostgreSQL connection string
- `SuperAdmin__Email`: Initial super admin email
- `SuperAdmin__Password`: Initial super admin password
- `AiModelService__BaseUrl`: Python FastAPI service URL

---

### Running with Docker Compose

The easiest way to get started is using **Docker Compose**, which orchestrates all services.

#### 1. Clone the Repository

```bash
git clone https://github.com/mosayass/career-path-backend.git
cd CareerPath_backend
```

#### 2. Start All Services

```bash
docker-compose up -d
```

This command will:
- Build the .NET application from the Dockerfile
- Start PostgreSQL database (on port 5433)
- Start Azurite (Azure Storage emulator on port 10000)
- Start the ASP.NET Core API (on port 8080)
- Create a shared Docker network for inter-service communication

#### 3. Verify Services are Running

```bash
docker-compose ps
```

Expected output:
```
NAME                    STATUS              PORTS
careerpath-db          Up 2 minutes         5432/tcp
careerpath-api         Up 2 minutes         0.0.0.0:8080->8080/tcp
azurite                Up 2 minutes         0.0.0.0:10000->10000/tcp
```

#### 4. Access the Application

- **Backend API**: http://localhost:8080
- **Swagger UI**: http://localhost:8080/swagger/index.html
- **Database**: PostgreSQL at localhost:5433 (User: postgres, Password: SuperSecretPassword123!)
- **Blob Storage**: Azurite at localhost:10000

#### 5. Stop All Services

```bash
docker-compose down
```

To also remove persistent data volumes:
```bash
docker-compose down -v
```

---

### Running Locally (Development)

For local development without Docker, follow these steps:

#### 1. Prerequisites

Ensure you have:
- .NET 10 SDK installed
- PostgreSQL running locally (or update connection string)
- Python FastAPI service running (for AI predictions)

#### 2. Install Dependencies

```bash
# Restore NuGet packages
dotnet restore
```

#### 3. Update Connection String

Edit `Host/appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=CareerPathDb;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

#### 4. Apply Database Migrations

```bash
cd Host

# Add and apply migrations
dotnet ef database update --project ../Modules/Identity/Infrastructure

dotnet ef database update --project ../Modules/Assessment/Infrastructure

dotnet ef database update --project ../Modules/Careers/Infrastructure

dotnet ef database update --project ../Modules/Community/Infrastructure

dotnet ef database update --project ../Modules/Profiles/Infrastructure
```

#### 5. Run the Application

```bash
cd Host
dotnet run
```

The API will be available at `http://localhost:8080`

#### 6. Access Swagger Documentation

Navigate to: http://localhost:8080/swagger/index.html

---

## AI Inference Integration

### Overview

The Assessment module integrates with a **Python FastAPI microservice** for AI-powered career predictions. When a user submits an assessment, the backend sends their responses to the Python service, which uses a machine learning model to predict the best-matching careers.

### How It Works

1. **User Submits Assessment**: User completes the assessment through the frontend
2. **Backend Receives Submission**: `POST /api/assessments/submit` endpoint processes the request
3. **AI Service Invoked**: `FastApiAiModelClient` calls the Python service's `/predict/top-matches` endpoint
4. **Predictions Generated**: ML model analyzes user responses and generates career matches
5. **Results Stored**: Predictions are saved to PostgreSQL database
6. **Results Returned**: Frontend displays career recommendations to the user

### Integration Details

**Endpoint**: `POST /predict/top-matches`

**Request Payload** (`AssessmentSubmissionPayload`):
```json
{
  "userId": "string",
  "responses": [
    {
      "questionId": 1,
      "selectedOptionId": 5
    },
    ...
  ]
}
```

**Response** (`AiPredictionResponse`):
```json
{
  "topMatches": [
    {
      "careerId": 1,
      "careerName": "Software Engineer",
      "matchScore": 0.95,
      "reasoning": "Strong match based on technical responses"
    },
    {
      "careerId": 2,
      "careerName": "Data Scientist",
      "matchScore": 0.88,
      "reasoning": "Good analytical skills detected"
    }
  ]
}
```

### Configuration

The AI service base URL is configured in `appsettings.json`:

```json
{
  "AiModelService": {
    "BaseUrl": "http://localhost:8000/"
  }
}
```

In Docker Compose, it's set to: `http://fast_inference_api:8000/`

### Starting the Python AI Service

Refer to the **[Career Path AI Inference](https://github.com/mosayass/career-path-ai-inference)** repository for setup instructions.

Quick start:
```bash
# Clone the AI service repository
git clone https://github.com/mosayass/career-path-ai-inference.git
cd career-path-ai-inference

# Install Python dependencies
pip install -r requirements.txt

# Run the FastAPI server
python -m uvicorn main:app --host 0.0.0.0 --port 8000
```

The service will be available at `http://localhost:8000` with API docs at `http://localhost:8000/docs`

---

## API Documentation

### Authentication

The API uses **JWT (JSON Web Tokens)** for authentication.

**Getting a Token**:
```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Using the Token**:
```bash
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Base URL

- **Development**: `http://localhost:8080`
- **Docker**: `http://localhost:8080`
- **Swagger UI**: `http://localhost:8080/swagger/index.html`

### Key Endpoints

#### Identity Module
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `POST /api/auth/refresh-token` - Refresh JWT token
- `POST /api/auth/forgot-password` - Initiate password reset
- `POST /api/auth/reset-password` - Reset password with token

#### Assessment Module
- `GET /api/assessments` - Get all assessments
- `POST /api/assessments/submit` - Submit assessment and get AI predictions
- `GET /api/assessments/{id}` - Get assessment details
- `GET /api/assessments/{id}/results` - Get assessment results and predictions

#### Career Module
- `GET /api/careers` - List all careers
- `GET /api/careers/{id}` - Get career details
- `GET /api/careers/search?keyword=...` - Search careers
- `GET /api/careers/{id}/requirements` - Get career requirements

#### Community Module
- `GET /api/posts` - Get community posts
- `POST /api/posts` - Create new post
- `PUT /api/posts/{id}` - Update post
- `DELETE /api/posts/{id}` - Delete post
- `POST /api/posts/{id}/comments` - Add comment to post

#### Profiles Module
- `GET /api/profiles/me` - Get current user profile
- `PUT /api/profiles/me` - Update user profile
- `GET /api/profiles/{userId}` - Get user profile by ID
- `GET /api/profiles/me/career-pathways` - Get career pathway history

For complete API documentation, access Swagger UI at: http://localhost:8080/swagger/index.html

---

## Database Migrations

### Creating a New Migration

```bash
cd Host

# Create a new migration for a specific module
dotnet ef migrations add MigrationName --project ../Modules/YourModule/Infrastructure --context YourModuleDbContext
```

### Applying Migrations

```bash
# Apply all pending migrations
dotnet ef database update --project ../Modules/YourModule/Infrastructure
```

### Viewing Migration Status

```bash
# List all migrations
dotnet ef migrations list --project ../Modules/YourModule/Infrastructure
```

### Reverting a Migration

```bash
# Revert to a specific migration
dotnet ef database update PreviousMigrationName --project ../Modules/YourModule/Infrastructure
```

---

## Docker Deployment

### Multi-Stage Build

The `Dockerfile` uses a **multi-stage build** for optimal image size and performance:

1. **base**: Lightweight ASP.NET Core runtime image
2. **build**: Full .NET SDK for building the application
3. **publish**: Creates the final published build
4. **final**: Production-ready image with only runtime dependencies

### Docker Compose Services

**docker-compose.yml** includes:

- **db**: PostgreSQL 16 Alpine
- **api**: ASP.NET Core application (built from Dockerfile)
- **azurite**: Azure Storage emulator for local blob storage
- **ai_shared_network**: Docker network for service communication

### Production Build

For production deployment, use `docker-compose.prod.yml`:

```bash
docker-compose -f docker-compose.prod.yml up -d
```

Update environment variables and connection strings for production:
```bash
# .env file or environment variables
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<production-db-string>
SuperAdmin__Email=admin@careerpath.com
SuperAdmin__Password=<strong-password>
```

---

## Troubleshooting

### Common Issues

**1. Database Connection Failed**
```
Error: could not connect to server: Connection refused
```
**Solution**: Ensure PostgreSQL is running and connection string is correct.

**2. AI Service Unavailable**
```
Error: AI prediction service returned an error: 503
```
**Solution**: Start the Python FastAPI service or check the BaseUrl configuration.

**3. Port Already in Use**
```
Error: Address already in use
```
**Solution**: Change ports in docker-compose.yml or stop conflicting services.

**4. Storage Connection Error**
```
Error: Cannot connect to Azurite
```
**Solution**: Ensure Azurite container is running and connection string matches configuration.

### Viewing Logs

```bash
# View logs for all services
docker-compose logs -f

# View logs for specific service
docker-compose logs -f api

# View logs with tail
docker-compose logs -f --tail=50
```

### Rebuilding Docker Images

```bash
# Rebuild without cache
docker-compose build --no-cache

# Rebuild specific service
docker-compose build --no-cache api
```

---

## Contributing

We welcome contributions! Please follow these steps:

1. **Fork the Repository**: https://github.com/mosayass/career-path-backend
2. **Create a Feature Branch**: `git checkout -b feature/your-feature-name`
3. **Commit Changes**: `git commit -m "Add your feature description"`
4. **Push to Branch**: `git push origin feature/your-feature-name`
5. **Open a Pull Request**: Describe your changes and submit for review

### Code Style Guidelines

- Follow C# coding conventions (PascalCase for classes, camelCase for variables)
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and single-responsibility
- Write unit tests for new features

---

## Project Structure

```
CareerPath_backend/
├── Host/                                  # Main application host
│   ├── CareerPath.Host.csproj
│   ├── Program.cs                         # Application startup & DI configuration
│   ├── Dockerfile                         # Docker image definition
│   └── appsettings.json                   # Configuration file
├── Modules/                               # Feature modules
│   ├── Assessment/                        # Assessment & AI integration
│   ├── Careers/                           # Career data
│   ├── Community/                         # Community features
│   ├── Identity/                          # Authentication & authorization
│   └── Profiles/                          # User profiles
├── Shared/                                # Shared infrastructure
│   ├── CareerPath.Shared/
│   ├── CareerPath.Shared.Api/
│   ├── CareerPath.Shared.Infrastructure/
│   └── CareerPath.Shared.IntegrationEvents/
├── docker-compose.yml                     # Development docker configuration
├── docker-compose.prod.yml                # Production docker configuration
└── README.md                              # This file
```

---

## License

This project is part of a graduation project. Check the repository for license details.

---

## Support & Contact

For questions or issues, please:
- Open an issue on GitHub: https://github.com/mosayass/career-path-backend/issues
- Contact the development team through the repository

---

## Additional Resources

- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Docker Documentation](https://docs.docker.com/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [FastAPI Documentation](https://fastapi.tiangolo.com/)

---
