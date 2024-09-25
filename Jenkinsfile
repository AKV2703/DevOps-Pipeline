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
                    // Build and publish the .NET application
                    sh 'dotnet build SimpleReactionMachine.sln'
                    sh 'dotnet publish SimpleReactionMachine.sln -c Release -o ./artifacts'
                    
                    // Build the Docker image
                    sh 'docker build -t simple-reaction-machine:latest .'
                    
                    // Archive Dockerfile and artifacts (optional)
                    archiveArtifacts artifacts: 'Dockerfile, artifacts/**', allowEmptyArchive: false
                }
            }
        }

        stage('Run Tests') {
            steps {
                script {
                    // Run unit tests using .NET CLI
                    sh 'dotnet test --logger "console;verbosity=detailed"'
                }
            }
        }

        stage('Code Quality Analysis') {
            steps {
                script {
                    // Perform static code analysis using SonarQube
                    withSonarQubeEnv('DevOps-Pipeline') {
                        sh 'dotnet sonarscanner begin /k:"DevOps-Pipeline" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="sqp_9b19ed1abea7ca2cd2323aa190d64cec4641f86e"'
                        sh 'dotnet build SimpleReactionMachine.sln'
                        sh 'dotnet sonarscanner end /d:sonar.login="sqp_9b19ed1abea7ca2cd2323aa190d64cec4641f86e"'
                    }
                }
            }
        }

        stage('Deploy to Test Environment') {
            steps {
                script {
                    // Stop and remove any existing containers
                    sh 'docker stop simple-reaction-machine-container || true'
                    sh 'docker rm simple-reaction-machine-container || true'
                    
                    // Deploy the Docker image to a test environment
                    sh 'docker run -d --name simple-reaction-machine-container -p 8081:80 simple-reaction-machine:latest'
                }
            }
        }

        stage('Release Docker Image') {
            steps {
                script {
                    // Tag the Docker image with the version for release
                    sh 'docker tag simple-reaction-machine:latest akv272003/simple-reaction-machine:1.0.$BUILD_ID'

                    // Log in to Docker Hub (if necessary, use Jenkins credentials plugin)
                    withCredentials([usernamePassword(credentialsId: 'docker-hub-creds', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
                        sh 'echo $DOCKER_PASS | docker login -u $DOCKER_USER --password-stdin'
                    }
                    
                    // Push the Docker image to Docker Hub
                    sh 'docker push akv272003/simple-reaction-machine:1.0.$BUILD_ID'
                    
                    // Optionally push the latest tag as well
                    sh 'docker push akv272003/simple-reaction-machine:latest'
                }
            }
        }

        stage('Deploy to Production') {
            steps {
                script {
                    // Stop and remove the existing container in production
                    sh 'docker stop simple-reaction-machine-container || true'
                    sh 'docker rm simple-reaction-machine-container || true'

                    // Pull and run the specific version from Docker Hub
                    sh 'docker pull akv272003/simple-reaction-machine:1.0.$BUILD_ID'
                    
                    // Run the new Docker image in the production environment
                    sh 'docker run -d --name simple-reaction-machine-container -p 8081:80 akv272003/simple-reaction-machine:1.0.$BUILD_ID'
                }
            }
        }
        stage('Cleanup Docker Images') {
            steps {
                script {
                    // Remove dangling Docker images (optional cleanup step)
                    sh 'docker image prune -f'
                }
            }
        }
    }
}