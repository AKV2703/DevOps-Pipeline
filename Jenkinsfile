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

                    // Check if Dockerfile exists in the root directory, then build the image
                    sh '''
                    if [ -f Dockerfile ]; then
                        docker build -t simple-reaction-machine:latest .
                    else
                        echo "Dockerfile not found!"
                        exit 1
                    fi
                    '''
                }
            }
        }

        stage('Archive Artifact') {
            steps {
                script {
                    // Archive Dockerfile and other build artifacts only if they exist
                    sh '''
                    if [ -f Dockerfile ]; then
                        archiveArtifacts artifacts: 'Dockerfile, artifacts/**', allowEmptyArchive: false
                    else
                        echo "No Dockerfile or artifacts to archive!"
                    fi
                    '''
                }
            }
        }
    }
}