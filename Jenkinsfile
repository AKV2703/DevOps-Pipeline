pipeline {
    agent any
    environment {
        // Define paths for Docker, .NET, and SonarScanner
        DOCKER_PATH = "/usr/local/bin"
        DOTNET_PATH = "/opt/homebrew/bin"
        DOTNET_TOOLS_PATH = "/Users/akv/.dotnet/tools"
        PATH = "${DOCKER_PATH}:${DOTNET_PATH}:${DOTNET_TOOLS_PATH}:$PATH"
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

        stage('Code Quality Analysis') {
            steps {
                script {
                    withSonarQubeEnv('DevOps-Pipeline') { // 'DevOps-Pipeline' is the name of SonarQube instance
                        sh 'dotnet sonarscanner begin /k:"DevOps-Pipeline" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="sqp_9b19ed1abea7ca2cd2323aa190d64cec4641f86e"'
                        sh 'dotnet build SimpleReactionMachine.sln'
                        sh 'dotnet sonarscanner end /d:sonar.login="sqp_9b19ed1abea7ca2cd2323aa190d64cec4641f86e"'
                    }
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    // Stop and remove the existing container if it exists
                    sh 'docker stop simple-reaction-machine-container || true'
                    sh 'docker rm simple-reaction-machine-container || true'
                    
                    // Deploy the Docker image to a test environment
                    // Running Docker container interactively, but keep in mind Jenkins may not handle the interactive part well
                    sh 'docker run -it --rm simple-reaction-machine:latest'
                    
                }
            }
        }
    }
}



