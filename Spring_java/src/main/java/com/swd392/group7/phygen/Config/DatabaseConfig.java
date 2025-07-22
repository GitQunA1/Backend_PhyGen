package com.swd392.group7.phygen.Config;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.datatype.jdk8.Jdk8Module;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.jdbc.core.JdbcTemplate;

import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.DatabaseMetaData;
import java.util.Objects;
import java.util.Optional;

@Configuration
public class DatabaseConfig {
    
    @Bean
    public JdbcTemplate jdbcTemplate(DataSource dataSource) {
        return new JdbcTemplate(dataSource);
    }

    @Bean
    public ObjectMapper objectMapper(){
        ObjectMapper objectMapper = new ObjectMapper();
        objectMapper.registerModule(new Jdk8Module());
        objectMapper.registerModule(new JavaTimeModule());
        return objectMapper;
    }

    @Bean
    public CommandLineRunner checkCurrentConnection(JdbcTemplate jdbcTemplate, MongoTemplate mongoTemplate) {
        return args -> {
            System.out.println("--- Kiểm tra kết nối PostgreSQL (Supabase) ---");
            try (Connection conn = Objects.requireNonNull(jdbcTemplate.getDataSource()).getConnection()) {
                DatabaseMetaData metaData = conn.getMetaData();
                String dbProduct = metaData.getDatabaseProductName();
                String dbVersion = metaData.getDatabaseProductVersion();

                System.out.println("✅ Connected to: " + dbProduct);
                System.out.println("🧠 Database Version: " + dbVersion);

                if (dbProduct.toLowerCase().contains("mysql")) {
                    System.out.println("🔁 Kết nối hiện tại là MySQL ✅");
                } else if (dbProduct.toLowerCase().contains("postgresql")) {
                    System.out.println("🔁 Kết nối hiện tại là PostgreSQL (Supabase) ✅");
                } else {
                    System.out.println("ℹ️ Hệ quản trị CSDL không xác định rõ");
                }

            } catch (Exception e) {
                System.out.println("❌ Lỗi khi kết nối CSDL SQL: " + e.getMessage());
            }

            System.out.println("\n--- Kiểm tra kết nối MongoDB ---");
            try {
                String databaseName = mongoTemplate.getDb().getName();


                mongoTemplate.executeCommand("{ ping: 1 }");

                System.out.println("✅ Connected to MongoDB Database: " + databaseName);
                System.out.println("🔁 Kết nối hiện tại là MongoDB Atlas ✅");

            } catch (Exception e) {
                System.out.println("❌ Lỗi khi kết nối MongoDB: " + e.getMessage());
                System.out.println("   Chi tiết lỗi: " + e.getClass().getSimpleName() + " - " + e.getMessage());
            }
        };
    }
}
