pipeline {
    agent any
    environment {
        // Define separate paths for Docker and .NET
        DOCKER_PATH = "/usr/local/bin"
        DOTNET_PATH = "/opt/homebrew/bin"
        // Combine both paths and the default PATH
        PATH = "${DOCKER_PATH}:${DOTNET_PATH}:$PATH"
        
        // Add the Code Climate Test Reporter ID here
        CC_TEST_REPORTER_ID = '239878133e2b6ae32aa6701c0f4d680ab58aa24a67cadefcb23b07e6791eac21'
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
        
        stage('Run Tests and Generate Coverage') {
            steps {
                script {
                    // Run tests with coverage enabled and output in Cobertura format
                    sh 'dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./TestResults/'

                }
            }
        }
        

}