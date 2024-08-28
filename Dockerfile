# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /source
COPY . .
RUN dotnet publish Web.csproj --configuration "Release" --output "dist"

# production stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /source/dist ./
ENV ASPNETCORE_URLS http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Web.dll"]