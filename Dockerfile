# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the solution file and the project file, and restore dependencies
COPY SimpleReactionMachine.sln ./
COPY SimpleReactionMachine/SimpleReactionMachine.csproj ./SimpleReactionMachine/
RUN dotnet restore

# Copy the rest of the application source code
COPY SimpleReactionMachine/ ./SimpleReactionMachine/
WORKDIR /app/SimpleReactionMachine

# Build and publish the app
RUN dotnet publish -c Release -o /app/out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "SimpleReactionMachine.dll"]



