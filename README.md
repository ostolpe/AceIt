# AceIt

A web app for practicing technical interview questions. Users log in, take a session where Claude generates questions based on selected topics, submit their answers, and get AI feedback on how they did. User can then see see their overall/topic-specific average score and see what they need to improve.  

## Stack

Backend is ASP.NET Core (.NET 10) with Entity Framework Core, JWT auth and PostgreSQL server running in docker. Frontend is React 19 with TypeScript and Vite. Feedback and scores are generated using the Anthropic API using their nuget package for integration.

## Getting started

Run the backend:

```
cd backend/AceIt/AceIt
dotnet run
```

Run the frontend:

```
cd frontend
npm install
npm run dev
```

You'll need a `.env` or `appsettings.Development.json` with your Anthropic API key and JWT secret.
