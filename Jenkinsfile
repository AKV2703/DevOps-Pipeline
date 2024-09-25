pipeline {
    agent any
    environment {
        // Define separate paths for Docker and .NET
        DOCKER_PATH = "/usr/local/bin"
        DOTNET_PATH = "/opt/homebrew/bin"
        // Combine both paths and the default PATH
        PATH = "${DOCKER_PATH}:${DOTNET_PATH}:${env.PATH}"
        
        // Add the Code Climate Test Reporter ID here
        CC_TEST_REPORTER_ID = '239878133e2b6ae32aa6701c0f4d680ab58aa24a67cadefcb23b07e6791eac21'
    }
    stages {
        stage('Build and Create Docker Image') {
            steps {
                script {
                    // Restore and Build the .NET project
                    sh 'dotnet restore SimpleReactionMachine.sln'
                    sh 'dotnet build SimpleReactionMachine.sln'
                    
                    // Publish the build for Docker
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
                    // Ensure the TestResults directory exists
                    sh 'mkdir -p ./SimpleReactionMachine/TestResults'

                    // Run tests with coverage enabled and output in Cobertura format
                    sh '''
                    dotnet test SimpleReactionMachine.sln \
                    /p:CollectCoverage=true \
                    /p:CoverletOutputFormat=cobertura \
                    /p:CoverletOutput=./SimpleReactionMachine/TestResults/
                    '''

                    // List files in TestResults to verify correct path
                    sh 'ls -l ./SimpleReactionMachine/TestResults/'

                    // Archive the test results from the correct path
                    archiveArtifacts artifacts: 'SimpleReactionMachine/TestResults/coverage.cobertura.xml', allowEmptyArchive: false
                }
            }
        }
                
        stage('Code Climate Test Coverage') {
            steps {
                script {
                    // Download and prepare the Code Climate Test Reporter
                    sh '''
                    curl -L https://codeclimate.com/downloads/test-reporter/test-reporter-latest-darwin-amd64 > ./cc-test-reporter
                    chmod +x ./cc-test-reporter
                    ./cc-test-reporter before-build
                    '''

                    // Format and upload the coverage report
                    sh """
                    ./cc-test-reporter format-coverage --input-type cobertura ./SimpleReactionMachine/TestResults/coverage.cobertura.xml \
                    --prefix \${WORKSPACE}/SimpleReactionMachine
                    """
                    sh './cc-test-reporter upload-coverage --id ${CC_TEST_REPORTER_ID}'
                }
            }
        }
    }
}