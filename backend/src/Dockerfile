FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ./ServerlessAPI /app/ServerlessAPI

WORKDIR /app/ServerlessAPI
RUN dotnet publish -c Release -o out

#só posso copiar aqui porque se eu copiar lá em cima ele vai ignorar o app.js
FROM node:16.0.0-alpine AS node_base
WORKDIR /app
COPY ./MathMLApi /app/MathMLApi
WORKDIR /app/MathMLApi
RUN npm install

# Create a startup script
COPY ./startup.sh /startup.sh
RUN chmod +x /startup.sh

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
#RUN apk add --no-cache icu-libs icu-data-full tzdata
COPY --from=node_base . .

WORKDIR /app
COPY --from=build /app/ServerlessAPI/out .
ENTRYPOINT ["/startup.sh"]