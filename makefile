.PHONY: dev stop
stop:
	-pkill -f vite
	-pkill -f "dotnet run"
dev: stop
	cd backend/AceIt/AceIt && dotnet run &
	cd frontend && npm run dev &
	trap 'make stop' INT; wait