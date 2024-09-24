pipeline {
    agent any
    stages {
        stage('Verify dotnet') {
            steps {
                sh 'dotnet --version'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build SimpleReactionMachine.sln'
            }
        }
    }
}