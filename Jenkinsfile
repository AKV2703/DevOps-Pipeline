pipeline {
    agent any
    environment {
        // Add the Docker path to the PATH environment variable
        PATH = "/usr/local/bin:$PATH"
    }
    stages {
        stage('Test Docker Access') {
            steps {
                // Check Docker version to ensure Docker is accessible
                sh 'docker --version'
            }
        }
    }
}