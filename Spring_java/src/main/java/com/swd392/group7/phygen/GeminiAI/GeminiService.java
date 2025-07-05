//package com.swd392.group7.phygen.GeminiAI;
//
//import dev.langchain4j.data.message.AiMessage;
//import dev.langchain4j.model.chat.ChatModel;
//import dev.langchain4j.model.googleai.GoogleAiGeminiChatModel;
//import lombok.RequiredArgsConstructor;
//import org.springframework.jdbc.core.JdbcTemplate;
//import org.springframework.stereotype.Service;
//
//import javax.sql.DataSource;
//import java.sql.Connection;
//import java.sql.DatabaseMetaData;
//import java.sql.ResultSet;
//import java.sql.SQLException;
//import java.util.ArrayList;
//import java.util.HashMap;
//import java.util.List;
//import java.util.Map;
//
//@Service
//@RequiredArgsConstructor
//public class GeminiService {
//
//    private final JdbcTemplate jdbcTemplate;
//
//    private final GoogleAiGeminiChatModel geminiChatModel;
//
//    private String schemaInfo() throws SQLException {
//        StringBuilder schema = new StringBuilder();
//        DataSource dataSource = jdbcTemplate.getDataSource();
//        if (dataSource == null) {
//            schema.append("DataSource is null. Check database configuration.");
//            return schema.toString();
//        }
//        try (Connection connection = dataSource.getConnection()) {
//            DatabaseMetaData metaData = connection.getMetaData();
//            ResultSet tables = metaData.getTables(null, null, "%", new String[]{"TABLE"});
//
//            while (tables.next()) {
//                String tableName = tables.getString("TABLE_NAME");
//                schema.append("Table: ").append(tableName).append(" (");
//
//                ResultSet columns = metaData.getColumns(null, null, tableName, "%");
//                List<String> columnNames = new ArrayList<>();
//                while (columns.next()) {
//                    columnNames.add(columns.getString("COLUMN_NAME"));
//                }
//                schema.append(String.join(", ", columnNames)).append(")\n");
//                columns.close();
//            }
//            tables.close();
//        } catch (Exception e) {
//            schema.append("Error reading schema: ").append(e.getMessage());
//        }
//        return schema.toString();
//    }
//
//    private List<Map<String, String>> exampleQueries() {
//        return List.of(
//                asQuery("Lấy danh sách câu trả lời đúng, chưa bị xóa",
//                        "SELECT * FROM answer WHERE is_correct = TRUE AND is_deleted = FALSE;"),
//                asQuery("Lấy danh sách ma trận kiểm tra giữa kỳ còn hoạt động\n",
//                        "SELECT * FROM exam_matrix\n" +
//                                "WHERE examtype = 'midterm' AND status = 'active';\n"),
//                asQuery("Lấy danh sách các phần của đề kiểm tra chưa bị xóa\n", "SELECT * FROM exam_section\n" +
//                        "WHERE is_deleted = FALSE;\n"),
//                asQuery("Lấy danh sách các phần của ma trận kiểm tra chưa bị xóa\n", "SELECT * FROM matrix_section\n" +
//                        "WHERE is_deleted = FALSE;\n"),
//                asQuery("Lấy danh sách chi tiết ma trận với độ khó trung bình và dạng trắc nghiệm", "SELECT * FROM matrix_detail\n" +
//                        "WHERE difficulty_level = 'medium' AND type = 'multiple_choice';\n"),
//                asQuery("Lấy câu hỏi tự luận, khó, đang hoạt động\n", "SELECT * FROM question\n" +
//                        "WHERE difficulty_level = 'hard' AND type = 'essay' AND status = 'active';\n" +
//                        "\n")
//        );
//    }
//
//    private Map<String, String> asQuery(String question, String query) {
//        Map<String, String> map = new HashMap<>();
//        map.put("Q", question);
//        map.put("A", query);
//        return map;
//    }
//
//    private String formatExamples(List<Map<String, String>> examples) {
//        StringBuilder sb = new StringBuilder();
//        for (Map<String, String> e : examples) {
//            sb.append("Q: ").append(e.get("Q").trim()).append("\n");
//            sb.append("A: ").append(e.get("A").trim()).append("\n\n");
//        }
//        return sb.toString();
//    }
//
//    private String buildPrompt(String userQuestion) {
//        try {
//            String schema = schemaInfo();
//            String examples = formatExamples(exampleQueries());
//
//            return String.format("""
//                    Bạn là chuyên gia SQL. Dưới đây là thông tin schema:
//                    %s
//
//                    Và đây là những câu query mẫu để bạn tham khảo:
//                    %s
//
//                    Lắng nghe câu hỏi người dùng để trả về câu trả lời SQL (chỉ trả về câu lệnh SQL, không cần giải thích):
//                    Q: %s
//                    A:
//                    """, schema, examples, userQuestion);
//
//
//        } catch (Exception e) {
//            return "Lỗi khi tạo prompt: " + e.getMessage();
//        }
//    }
//
//    public Object handleUserQuery(String userQuestion) {
//        String prompt = buildPrompt(userQuestion);
//
//        String sql = "";
//        int maxTries = 3;
//        int currTry = 0;
//        Exception lastException = null;
//        List<Map<String, Object>> data = null;
//
//        // Vòng lặp: nếu SQL lỗi thì refine lại prompt
//        while (currTry < maxTries) {
//            currTry++;
//            try {
//                AiMessage response = geminiChatModel.
//                // Có thể kiểm tra chuỗi SQL có hợp lệ không, loại bỏ dấu ```sql hoặc ký tự thừa
//                sql = cleanSql(sql);
//
//                // Thực thi SQL
//                data = jdbcTemplate.queryForList(sql);
//                // Nếu thành công, trả kết quả
//                return data;
//            } catch (Exception e) {
//                lastException = e;
//                // Nếu lỗi, cập nhật prompt cho lần tiếp theo
//                prompt = buildPromptWithError(userQuestion, sql, e.getMessage());
//            }
//        }
//        // Nếu thử nhiều lần vẫn lỗi, trả lỗi ra ngoài
//        return "Không thể sinh truy vấn SQL hợp lệ. Lỗi cuối cùng: " + (lastException != null ? lastException.getMessage() : "Unknown error") + "\nSQL: " + sql;
//    }
//
//    // Tiện ích: Làm sạch chuỗi SQL nếu Gemini trả về thừa ký tự
//    private String cleanSql(String sql) {
//        sql = sql.replaceAll("```sql", "").replaceAll("```", "").trim();
//        // Có thể thêm logic xóa giải thích nếu có
//        int semicolonIdx = sql.indexOf(";");
//        if (semicolonIdx != -1 && semicolonIdx < sql.length() - 1) {
//            sql = sql.substring(0, semicolonIdx + 1); // Lấy đến dấu ;
//        }
//        return sql;
//    }
//
//
//}
