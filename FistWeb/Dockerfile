# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["FistWeb.csproj", "./"]
RUN dotnet restore "FistWeb.csproj"

COPY . .

# 👉 Build cho Linux thay vì mặc định (Windows)
RUN dotnet publish "FistWeb.csproj" -c Release -r linux-x64 --self-contained false -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

#EXPOSE 80
#ENTRYPOINT ["dotnet", "FistWeb.dll", "--urls", "http://+:80"]
ENTRYPOINT ["dotnet", "FistWeb.dll"]