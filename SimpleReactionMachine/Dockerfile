# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application and build
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port on which the app will run
EXPOSE 80

# Command to run the application
ENTRYPOINT ["dotnet", "SimpleReactionMachine.dll"]