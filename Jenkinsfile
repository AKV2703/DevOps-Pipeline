pipeline {
    agent any
    environment {
        // Define paths for Docker and .NET
        DOCKER_PATH = "/usr/local/bin"
        DOTNET_PATH = "/opt/homebrew/bin"
        PATH = "${DOCKER_PATH}:${DOTNET_PATH}:$PATH"
    }
    stages {
        stage('Build and Create Docker Image') {
            steps {
                script {
                    sh 'dotnet build SimpleReactionMachine.sln'
                    sh 'dotnet publish SimpleReactionMachine.sln -c Release -o ./artifacts'
                    sh 'docker build -t simple-reaction-machine:latest .'
                    archiveArtifacts artifacts: 'Dockerfile, artifacts/**', allowEmptyArchive: false
                }
            }
        }
        
        stage('Run Tests') {
            steps {
                script {
                    sh 'dotnet test --logger "console;verbosity=detailed"'
                }
            }
        }


    }
}