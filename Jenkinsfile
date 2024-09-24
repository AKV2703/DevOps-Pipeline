pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                // Use MSBuild or dotnet CLI to build the project
                sh 'dotnet build SimpleReactionMachine.sln'
            }
        }
        stage('Test') {
            steps {
                // Run tests using NUnit or another test framework
                sh 'dotnet test'
            }
        }
        stage('Deploy') {
            steps {
                // You can define deployment steps here, for example deploying to a server
                echo 'Deploying the application...'
            }
        }
    }
}