FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY ServerlessAPI/. ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build /app/out/ /var/task/
#COPY app/out/ /var/task
RUN ls -la /var/task
CMD ["dotnet", "/var/task/ServerlessAPI.dll"]