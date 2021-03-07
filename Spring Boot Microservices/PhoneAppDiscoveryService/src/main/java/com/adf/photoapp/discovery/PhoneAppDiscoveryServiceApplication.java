package com.adf.photoapp.discovery;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.netflix.eureka.server.EnableEurekaServer;

@SpringBootApplication
@EnableEurekaServer()
public class PhoneAppDiscoveryServiceApplication {

	public static void main(String[] args) {
		SpringApplication.run(PhoneAppDiscoveryServiceApplication.class, args);
	}
}
