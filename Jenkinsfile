pipeline {
    agent any
    environment {
        // Define separate paths for Docker and .NET
        DOCKER_PATH = "/usr/local/bin"
        DOTNET_PATH = "/opt/homebrew/bin"
        // Combine both paths and the default PATH
        PATH = "${DOCKER_PATH}:${DOTNET_PATH}:$PATH"
    }
    stages {
        stage('Build and Create Docker Image') {
            steps {
                script {
                    
                    sh 'dotnet build SimpleReactionMachine.sln'
                    // Build the .NET project
                    sh 'dotnet publish SimpleReactionMachine.sln -c Release -o ./artifacts'
                    
                    // Build the Docker image only if the .NET build was successful
                    sh 'docker build -t simple-reaction-machine:latest .'

                    // Archive the Dockerfile and other important build artifacts
                    archiveArtifacts artifacts: 'Dockerfile, artifacts/**', allowEmptyArchive: false
                }
            }
        }
        
        stage('Run Tests') {
            steps {
                script {
                    // Run the main solution file which contains Tester.cs
                    sh 'dotnet run --project SimpleReactionMachine/SimpleReactionMachine.csproj'
                }
            }
        }

    }
}