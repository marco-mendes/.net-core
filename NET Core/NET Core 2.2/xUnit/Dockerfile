FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /src

# Copiar csproj e restaurar dependencias
COPY . ./
RUN dotnet restore
RUN dotnet test
RUN dotnet build
RUN dotnet publish "Terminal/Terminal.csproj" -c Release -o out

# Build da imagem
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /src/Terminal/out .
ENTRYPOINT ["dotnet", "Terminal.dll"]

