package com.swd392.group7.phygen.Config;

import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.jdbc.core.JdbcTemplate;

import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.DatabaseMetaData;
import java.util.Objects;

@Configuration
public class DatabaseConfig {
    
    @Bean
    public JdbcTemplate jdbcTemplate(DataSource dataSource) {
        return new JdbcTemplate(dataSource);
    }

    @Bean
    public CommandLineRunner checkCurrentConnection(JdbcTemplate jdbcTemplate) {
        return args -> {
            try (Connection conn = Objects.requireNonNull(jdbcTemplate.getDataSource()).getConnection()) {
                DatabaseMetaData metaData = conn.getMetaData();
                String dbProduct = metaData.getDatabaseProductName();
                String dbVersion = jdbcTemplate.queryForObject("SELECT version()", String.class);

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
                System.out.println("❌ Lỗi khi kết nối CSDL: " + e.getMessage());
            }
        };

    }
}
