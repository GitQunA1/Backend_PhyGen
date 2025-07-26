package com.swd392.group7.phygen;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.enums.SecuritySchemeIn;
import io.swagger.v3.oas.annotations.enums.SecuritySchemeType;
import io.swagger.v3.oas.annotations.info.Info;
import io.swagger.v3.oas.annotations.security.SecurityScheme;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@SpringBootApplication
@OpenAPIDefinition(info =  @Info(title = "PhyGen API", version ="1.0",description = "Information"))
@SecurityScheme(name="api",scheme = "bearer", type= SecuritySchemeType.HTTP, in = SecuritySchemeIn.HEADER)
@EnableScheduling
public class PhyGenApplication {

	public static void main(String[] args) {
		SpringApplication.run(PhyGenApplication.class, args);
	}

}
