# CloudNativeApplication
Sample application for creating a cloud native system utilizing spring cloud and steeltoe.net 

# Udemy Course for learning spring cloud
https://www.udemy.com/course/spring-boot-microservices-and-spring-cloud/

# Steeltoe
https://steeltoe.io/

# Pre-requisites 
- .Net Core 3.1
- JDK 10
- DockerHub
- Spring Tool Suite 4
- Visual Studio 19

# How to start Docker Container containing Elastic Search, MongoDb and Kibana
Open the commandline to the and goto the folder ..\CloudNativeApplication\NetMicroservice and write the command

```bash
$ docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
 ```

# How to start Docker Container for Zipkin
Open the commandline to the and goto the folder ..\CloudNativeApplication\Zipkin and write the command

```bash
$ docker-compose -f docker-compose-slim.yml up
```

# Steps for starting the application
- Start the Eureka Server from the Spring Tool Suite
- Start the docker container for MongoDb, Elastic Search, Kibana and the container for the Zipkin server
- Start the microservices from both .net and spring tool suite to register on Eureka
- Start the Api Gateway to register on Eureka

