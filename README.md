# AceIt

A web app for practicing technical interview questions. Users log in, take a session where Claude generates questions based on selected topics, submit their answers, and get AI feedback on how they did.

## Stack

Backend is ASP.NET Core (.NET 10) with Entity Framework Core, PostgreSQL server running in docker and JWT auth. Frontend is React 19 with TypeScript and Vite. Feedback and scores are generated using the Anthropic API.

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
