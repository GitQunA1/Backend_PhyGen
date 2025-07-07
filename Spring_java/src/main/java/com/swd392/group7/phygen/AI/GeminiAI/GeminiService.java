package com.swd392.group7.phygen.AI.GeminiAI;

import com.google.genai.Client;
import com.google.genai.types.GenerateContentResponse;
import lombok.RequiredArgsConstructor;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.DatabaseMetaData;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class GeminiService {

    private final JdbcTemplate jdbcTemplate;

    private final Client client;


    public String generateExam(GenerateExamRequest request){
        String prompt = PromptBuilder.buildExamPrompt(request);
        GenerateContentResponse response = client.models.generateContent(
                "gemini-2.5-flash",
                prompt,
                null
        );
        return response.text();
    }


    private String schemaInfo() throws SQLException {
        StringBuilder schema = new StringBuilder();
        DataSource dataSource = jdbcTemplate.getDataSource();
        if (dataSource == null) {
            schema.append("DataSource is null. Check database configuration.");
            return schema.toString();
        }
        try (Connection connection = dataSource.getConnection()) {
            DatabaseMetaData metaData = connection.getMetaData();
            ResultSet tables = metaData.getTables(null, null, "%", new String[]{"TABLE"});

            while (tables.next()) {
                String tableName = tables.getString("TABLE_NAME");
                schema.append("Table: ").append(tableName).append(" (");

                ResultSet columns = metaData.getColumns(null, null, tableName, "%");
                List<String> columnNames = new ArrayList<>();
                while (columns.next()) {
                    columnNames.add(columns.getString("COLUMN_NAME"));
                }
                schema.append(String.join(", ", columnNames)).append(")\n");
                columns.close();
            }
            tables.close();
        } catch (Exception e) {
            schema.append("Error reading schema: ").append(e.getMessage());
        }
        return schema.toString();
    }


}
