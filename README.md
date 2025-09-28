# CommBank Server Setup

## Prerequisites

- .NET 9.0 SDK
- MongoDB Atlas account

## Setup Instructions

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd CommBank-Server
   ```

2. **Configure MongoDB Connection**

   - Copy `CommBank-Server/Secrets.json.template` to `CommBank-Server/Secrets.json`
   - Replace the placeholder values in `Secrets.json` with your actual MongoDB Atlas connection details:
     - `YOUR_USERNAME`: Your MongoDB Atlas database username
     - `YOUR_PASSWORD`: Your MongoDB Atlas database password
     - `YOUR_CLUSTER`: Your MongoDB Atlas cluster name
     - `YOUR_APP_NAME`: Your application name (optional)

3. **Install Dependencies**

   ```bash
   dotnet restore
   ```

4. **Build the Application**

   ```bash
   dotnet build
   ```

5. **Run Tests**

   ```bash
   dotnet test
   ```

6. **Run the Application**

   ```bash
   cd CommBank-Server
   dotnet run
   ```

7. **Seed the Database (Optional)**
   ```bash
   curl -X POST http://localhost:11366/api/seed/all
   ```

## Features Added

- ✅ Icon field added to Goal model
- ✅ Database seeding functionality
- ✅ Comprehensive test coverage for GetGoalsForUser route
- ✅ MongoDB Atlas integration

## API Endpoints

- `GET /api/goal` - Get all goals
- `GET /api/goal/{id}` - Get goal by ID
- `GET /api/goal/User/{id}` - Get goals for specific user
- `POST /api/seed/all` - Seed database with sample data
- `GET /api/seed/status` - Check database seed status

## Security Notes

- Never commit `Secrets.json` to version control
- Use environment variables or secure secret management in production
- The `Secrets.json` file is ignored by git to prevent credential exposure
