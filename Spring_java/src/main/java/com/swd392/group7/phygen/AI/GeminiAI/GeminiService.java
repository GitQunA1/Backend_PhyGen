package com.swd392.group7.phygen.AI.GeminiAI;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;
import com.google.genai.Client;
import com.google.genai.types.GenerateContentConfig;
import com.google.genai.types.GenerateContentResponse;
import com.google.genai.types.Schema;
import com.swd392.group7.phygen.AI.MistralAI.MistralAIService;
import com.swd392.group7.phygen.AI.Structure.Dto.GenerateExamRequest;
import com.swd392.group7.phygen.AI.Structure.Dto.SubjectDto;
import com.swd392.group7.phygen.AI.Structure.Prompt.PromptBuilder;
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
import java.util.regex.Matcher;
import java.util.regex.Pattern;

@Service
@RequiredArgsConstructor
public class GeminiService {

    private final JdbcTemplate jdbcTemplate;

    private final Client client;

    private final MistralAIService mistralAIService;

    private final PromptBuilder promptBuilder;

    private final ObjectMapper objectMapper;


    public String generateExam(GenerateExamRequest request){
        String prompt = promptBuilder.buildGenerateExamPrompt(request);
        GenerateContentResponse response = client.models.generateContent(
                "gemini-2.5-flash",
                prompt,
                null
        );
        return response.text();
    }

    public String formatJson(String fileUrl) {
        String document = mistralAIService.importDocUrl(fileUrl);
        String prompt = promptBuilder.buildFormatJsonPrompt(document);
        try {
            ObjectNode configNode = objectMapper.createObjectNode();
            configNode.put("responseMimeType", "application/json");
            String finalConfigJsonString = objectMapper.writeValueAsString(configNode);
            GenerateContentConfig generationConfig = GenerateContentConfig.fromJson(finalConfigJsonString);
            GenerateContentResponse response = client.models.generateContent(
                    "gemini-2.5-flash",
                    prompt,
                    null
            );
            String extractJson = extractJsonFromMarkdown(response.text());
            System.out.println("✅ JSON từ Gemini:\n" + extractJson);
            return extractJson;
        } catch (Exception e) {
            System.err.println("❌ Lỗi khi format JSON với Gemini API: " + e.getMessage());
            e.printStackTrace();
            return "{}";
        }
    }


    private String extractJsonFromMarkdown(String text){
        Pattern pattern = Pattern.compile("```json\\s*([\\s\\S]*?)\\s*```");
        Matcher matcher = pattern.matcher(text);
        if (matcher.find()) {
            return matcher.group(1).trim();
        }
        String processedText = text.trim();
        if ((processedText.startsWith("{") && processedText.endsWith("}")) || (processedText.startsWith("[") && processedText.endsWith("]"))) {
            return processedText;
        }
        return "";
    }


    public String schemaInfo() throws SQLException {
        StringBuilder schema = new StringBuilder();
        DataSource dataSource = jdbcTemplate.getDataSource();
        if (dataSource == null) {
            schema.append("DataSource is null. Check database configuration.");
            return schema.toString();
        }
        try (Connection connection = dataSource.getConnection()) {

            String currentDb = connection.getCatalog();
            schema.append("🔍 Using database (schema): ").append(currentDb).append("\n\n");

            DatabaseMetaData metaData = connection.getMetaData();
            ResultSet tables = metaData.getTables(currentDb, null, "%", new String[]{"TABLE"});

            while (tables.next()) {
                String tableName = tables.getString("TABLE_NAME");
                schema.append("Table: ").append(tableName).append("/n");

                ResultSet columns = metaData.getColumns(null, null, tableName, "%");
                List<String> columnNames = new ArrayList<>();
                while (columns.next()) {
                    columnNames.add(columns.getString("COLUMN_NAME"));
                }
                schema.append("   ➤ Columns:(").append(String.join(", ", columnNames)).append(")\n\n");
                columns.close();
            }
            tables.close();
        } catch (Exception e) {
            schema.append("Error reading schema: ").append(e.getMessage());
        }
        return schema.toString();
    }


}
