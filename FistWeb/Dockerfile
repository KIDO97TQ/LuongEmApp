# Dockerfile ở thư mục FistWeb/

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Do Dockerfile đang trong FistWeb, file .csproj cũng ở đây,
# nên dùng đường tương đối đúng (không thêm "FistWeb/" phía trước)
COPY ["FistWeb.csproj", "./"]
RUN dotnet restore "FistWeb.csproj"

COPY . .   # copy toàn bộ nội dung thư mục FistWeb vào /app

RUN dotnet publish "FistWeb.csproj" -c Release -r linux-x64 --self-contained false -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "FistWeb.dll", "--urls", "http://+:80"]
