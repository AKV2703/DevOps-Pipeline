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
                    // Build the .NET project
                    sh 'dotnet build SimpleReactionMachine.sln'
                    sh 'dotnet publish SimpleReactionMachine.sln -c Release -o ./artifacts'
                    
                    // Build the Docker image
                    sh 'docker build -t simple-reaction-machine:latest .'
                    
                    // Archive the Dockerfile and artifacts
                    archiveArtifacts artifacts: 'Dockerfile, artifacts/**', allowEmptyArchive: false
                }
            }
        }
        
        stage('Run Tests') {
            steps {
                script {
                    // Run the tests and log detailed output in console
                    sh 'dotnet test --logger "console;verbosity=detailed"'
                }
            }
        }

    }
}