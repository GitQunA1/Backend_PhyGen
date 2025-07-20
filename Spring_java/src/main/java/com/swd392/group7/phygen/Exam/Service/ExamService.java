package com.swd392.group7.phygen.Exam.Service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.openhtmltopdf.pdfboxout.PdfRendererBuilder;
import com.swd392.group7.phygen.AI.GeminiAI.GeminiService;
import com.swd392.group7.phygen.AI.Structure.Prompt.PromptBuilder;
import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Account.Service.AccountService;
import com.swd392.group7.phygen.Answer.Dto.AnswerStructureResponse;
import com.swd392.group7.phygen.Answer.Entity.Answer;
import com.swd392.group7.phygen.Answer.Exception.AnswerException;
import com.swd392.group7.phygen.Answer.Repository.AnswerRepo;
import com.swd392.group7.phygen.Common.ByteArrayMultipartFile;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Dto.*;
import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.Exam.Enum.ExamType;
import com.swd392.group7.phygen.Exam.Enum.Status;
import com.swd392.group7.phygen.Exam.Exception.ExamException;
import com.swd392.group7.phygen.Exam.Repository.ExamRepo;
import com.swd392.group7.phygen.ExamMatrix.Dto.ExamMatrixStructureResponse;
import com.swd392.group7.phygen.ExamMatrix.Entity.ExamMatrix;
import com.swd392.group7.phygen.ExamMatrix.Exception.ExamMatrixException;
import com.swd392.group7.phygen.ExamMatrix.Repository.ExamMatrixRepo;
import com.swd392.group7.phygen.ExamQuestion.Dto.ExamQuestionResponse;
import com.swd392.group7.phygen.ExamQuestion.Entity.ExamQuestion;
import com.swd392.group7.phygen.ExamQuestion.Exception.ExamQuestionException;
import com.swd392.group7.phygen.ExamQuestion.Repository.ExamQuestionRepo;
import com.swd392.group7.phygen.ExamSection.Dto.ExamSectionCreateRequest;
import com.swd392.group7.phygen.ExamSection.Dto.ExamSectionFormatResponse;
import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.ExamSection.Exception.ExamSectionException;
import com.swd392.group7.phygen.ExamSection.Repository.ExamSectionRepo;
import com.swd392.group7.phygen.ExamSection.Service.ExamSectionService;
import com.swd392.group7.phygen.ExamVersion.Dto.ExamVersionGenerateResponse;
import com.swd392.group7.phygen.MatrixDetail.Dto.MatrixDetailStructureResponse;
import com.swd392.group7.phygen.MatrixDetail.Enum.DifficultyLevel;
import com.swd392.group7.phygen.MatrixDetail.Exception.MatrixDetailException;
import com.swd392.group7.phygen.MatrixDetail.MatrixDetail;
import com.swd392.group7.phygen.MatrixDetail.Repository.MatrixDetailRepo;
import com.swd392.group7.phygen.MatrixSection.Dto.MatrixSectionStructureResponse;
import com.swd392.group7.phygen.MatrixSection.Exception.MatrixSectionException;
import com.swd392.group7.phygen.MatrixSection.MatrixSection;
import com.swd392.group7.phygen.MatrixSection.Repository.MatrixSectionRepo;
import com.swd392.group7.phygen.Question.Dto.QuestionStructureResponse;
import com.swd392.group7.phygen.Question.Entity.Question;
import com.swd392.group7.phygen.Question.Exception.QuestionException;
import com.swd392.group7.phygen.Question.Repository.QuestionRepo;
import com.swd392.group7.phygen.Subject.Dto.SubjectStructureResponse;
import com.swd392.group7.phygen.Subject.Entity.Subject;
import com.swd392.group7.phygen.Subject.Service.SubjectService;
import com.swd392.group7.phygen.Topic.Dto.TopicStructureResponse;
import com.swd392.group7.phygen.Topic.Entity.Topic;
import com.swd392.group7.phygen.Topic.Enum.Level;
import com.swd392.group7.phygen.Topic.Exception.TopicException;
import com.swd392.group7.phygen.Topic.Repository.TopicRepo;
import com.vladsch.flexmark.html.HtmlRenderer;
import com.vladsch.flexmark.parser.Parser;
import lombok.RequiredArgsConstructor;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.modelmapper.ModelMapper;
import org.springframework.core.io.ClassPathResource;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import org.thymeleaf.TemplateEngine;
import org.thymeleaf.context.Context;

import java.io.ByteArrayOutputStream;
import java.io.InputStream;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class ExamService {
    private final AccountService accountService;
    private final ExamRepo examRepo;
    private final ModelMapper modelMapper;
    private final SubjectService subjectService;
    private final ExamMatrixRepo examMatrixRepo;
    private final MatrixSectionRepo matrixSectionRepo;
    private final MatrixDetailRepo matrixDetailRepo;
    private final TopicRepo topicRepo;
    private final QuestionRepo questionRepo;
    private final AnswerRepo answerRepo;
    private final ExamSectionRepo examSectionRepo;
    private final ExamQuestionRepo examQuestionRepo;
    private final ObjectMapper objectMapper;
    private final PromptBuilder promptBuilder;
    private final GeminiService geminiService;
    private final ExamSectionService examSectionService;
    private final TemplateEngine templateEngine;
    private final Parser markdownParser;
    private final HtmlRenderer htmlRenderer;

    public Exam findExamById(Long id) {
        return examRepo.findById(id).orElseThrow(() -> new ExamException("Cant found that Exam", HttpStatus.NOT_FOUND));
    }

    public ExamVersionGenerateResponse create(ExamCreateRequest examRq) throws JsonProcessingException {
        Account account = accountService.getCurrentAccount();
        Exam exam = new Exam();
        exam.setAccount(account);
        ExamType targetType = ExamType.valueOf(examRq.getExamType().name());
        exam.setExamType(targetType);
        exam.setCreatedAt(LocalDateTime.now());
        exam.setStatus(Status.active);
        exam.setDraft(true);
        examRepo.save(exam);

        ExamSectionCreateRequest examSectionCreateRequest = new ExamSectionCreateRequest();
        List<String> hardcodedSectionNames = new ArrayList<>();
        hardcodedSectionNames.add("Phần I:  Trắc nghiệm");
        hardcodedSectionNames.add("Phần II: Tự luận");
        examSectionCreateRequest.setSectionName(hardcodedSectionNames);
        examSectionService.create(examSectionCreateRequest, exam.getId());

        ExamFormatResponse examFormatResponse = getListExamFormat(exam.getId());
        ExamStructureRequest examStructureRequest = new ExamStructureRequest();
        examStructureRequest.setExamType(examRq.getExamType());
        examStructureRequest.setSubjectId(examRq.getSubjectId());
        examStructureRequest.setDifficultLevel(examRq.getDifficultyLevel());
        ExamStructureResponse examStructureResponse = getExamStructure(examStructureRequest);

        String examFormatJson = objectMapper.writeValueAsString(examFormatResponse);
        String examStructureJson = objectMapper.writeValueAsString(examStructureResponse);

        String examTemplate = promptBuilder
                .buildGenerateExamPrompt()
                .replace("{{EXAM_FORMAT_RESPONSE_JSON}}", examFormatJson)
                .replace("{{EXAM_STRUCTURE_RESPONSE_JSON}}", examStructureJson)
                .replace("{{EXAM_ID_PLACEHOLDER}}", exam.getId().toString())
                .replace("{{NUMBER_OF_EXAMS_TO_GENERATE}}", examRq.getExamQuantity());

        String examGeminiGenerate = geminiService.generateExam(examTemplate);
        return objectMapper.readValue(examGeminiGenerate, ExamVersionGenerateResponse.class);
    }

    public ExamFormatResponse getListExamFormat(Long examId) {
        Exam exam = findExamById(examId);
        ExamResponse examResponse = modelMapper.map(exam, ExamResponse.class);

        ExamFormatResponse examFormatResponse = new ExamFormatResponse();
        examFormatResponse.setExamResponse(examResponse);
        examFormatResponse.setExamSectionFormatResponses(new ArrayList<>());

        List<ExamSection> examSections = examSectionRepo.findAllByExamId(exam.getId()).orElseThrow(() -> new ExamSectionException("Empty list exam section", HttpStatus.NOT_FOUND));

        for (ExamSection examSection : examSections) {
            ExamSectionFormatResponse examSectionFormatResponse = modelMapper.map(examSection, ExamSectionFormatResponse.class);
            examSectionFormatResponse.setExamQuestionResponses(new ArrayList<>());
            List<ExamQuestion> examQuestions = examQuestionRepo.findAllByExamSectionId(examSection.getId()).orElseThrow(() -> new ExamQuestionException("Empty list exam question", HttpStatus.NOT_FOUND));
            for (ExamQuestion examQuestion : examQuestions) {
                ExamQuestionResponse examQuestionResponse = modelMapper.map(examQuestion, ExamQuestionResponse.class);

                examSectionFormatResponse.getExamQuestionResponses().add(examQuestionResponse);
            }
            examFormatResponse.getExamSectionFormatResponses().add(examSectionFormatResponse);
        }
        return examFormatResponse;
    }

    public ExamResponse update(ExamUpdateRequest examRq, Long id) {
        Exam oldExam = findExamById(id);
        oldExam.setExamType(examRq.getExamType());
        oldExam.setCreatedAt((examRq.getCreateAt()));
        examRepo.save(oldExam);
        return modelMapper.map(oldExam, ExamResponse.class);
    }

    public void deleted(Long id) {
        Exam exam = findExamById(id);
        exam.setDraft(true);
        examRepo.save(exam);
    }

    public PagedResponse<ExamResponse> getAllExamFromAccountId(
            int page,
            int size,
            String sortBy,
            String sortDir) {
        Account account = accountService.getCurrentAccount();
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));

        Page<Exam> examPage = examRepo.findAllByAccountId(account.getId(), pageRequest);
        if (examPage.isEmpty()) {
            throw new ExamException("Empty list exam for accountId: " + account.getId(), HttpStatus.NOT_FOUND);
        }
        List<ExamResponse> responses = examPage.stream().map(exam -> modelMapper.map(exam, ExamResponse.class)).toList();
        return new PagedResponse<>(
                responses,
                page,
                size,
                examPage.getTotalElements(),
                examPage.getTotalPages(),
                examPage.isLast()
        );
    }

    public ExamStructureResponse getExamStructure(ExamStructureRequest request) {
        Level level = Level.valueOf(request.getDifficultLevel().name());

        Subject subject = subjectService.findSubjectById(request.getSubjectId());
        SubjectStructureResponse subjectStructureResponse = modelMapper.map(subject, SubjectStructureResponse.class);


        ExamMatrix examMatrix = examMatrixRepo.findBySubjectIdAndExamType(request.getSubjectId(), request.getExamType()).orElseThrow(() -> new ExamMatrixException("Empty list exam matrix", HttpStatus.NOT_FOUND));

        ExamMatrixStructureResponse examMatrixStructureResponse = modelMapper.map(examMatrix, ExamMatrixStructureResponse.class);
        examMatrixStructureResponse.setMatrixSectionStructureResponses(new ArrayList<>());

        List<MatrixSection> matrixSections = matrixSectionRepo.findAllByExamMatrixId(examMatrix.getId()).orElseThrow(() -> new MatrixSectionException("Empty list matrix section", HttpStatus.NOT_FOUND));

        for (MatrixSection matrixSection : matrixSections) {
            MatrixSectionStructureResponse matrixSectionStructureResponse = modelMapper.map(matrixSection, MatrixSectionStructureResponse.class);
            matrixSectionStructureResponse.setMatrixDetailStructureResponseList(new ArrayList<>());

            List<MatrixDetail> matrixDetails = matrixDetailRepo.findAllByMatrixSectionIdAndDifficultyLevel(matrixSection.getId(), request.getDifficultLevel()).orElseThrow(() -> new MatrixDetailException("Empty list matrix detail", HttpStatus.NOT_FOUND));

            for (MatrixDetail matrixDetail : matrixDetails) {
                MatrixDetailStructureResponse matrixDetailStructureResponse = modelMapper.map(matrixDetail, MatrixDetailStructureResponse.class);


                Topic rootTopic = topicRepo.findTopicByIdAndLevel(matrixDetail.getTopic().getId(), level).orElseThrow(() -> new TopicException("Cant found topic with level " + request.getDifficultLevel().toString(), HttpStatus.NOT_FOUND));
                TopicStructureResponse rootTopicStructureResponse = buildTopicTree(rootTopic, request.getDifficultLevel());

                TopicStructureResponse topicStructureResponse = modelMapper.map(rootTopicStructureResponse, TopicStructureResponse.class);
                topicStructureResponse.setQuestionStructureResponses(new ArrayList<>());


                matrixDetailStructureResponse.setTopicStructureResponse(topicStructureResponse);
                matrixSectionStructureResponse.getMatrixDetailStructureResponseList().add(matrixDetailStructureResponse);
            }
            examMatrixStructureResponse.getMatrixSectionStructureResponses().add(matrixSectionStructureResponse);
        }
        subjectStructureResponse.setExamMatrixStructureResponse(examMatrixStructureResponse);
        return new ExamStructureResponse(subjectStructureResponse);
    }

    public MultipartFile convertMarkdownToPdf(String markdown,String key){
        String reformatMarkdown = markdown
                .replace("\\\"", "\"")
                .replace("\\n", "\n")
                .replace("\\", "")
                .replaceAll("---\\s*(PHẦN.*?)\\s*---", "$1")
                .replaceAll("(Lớp:\\s*\\d+)\\s+(Môn:\\s*[^\\n]+)\\s+(Thời gian:\\s*[^\\n]+)", "$1\n\n$2\n\n$3")
                .replaceAll("(Câu \\d+:.*?)\\s*A\\.", "$1\n\nA.")
                .replaceAll("A\\.(.*?)\\s*B\\.", "A.$1\n\nB.")
                .replaceAll("B\\.(.*?)\\s*C\\.", "B.$1\n\nC.")
                .replaceAll("C\\.(.*?)\\s*D\\.", "C.$1\n\nD.")
                .replaceAll("(Mã đề:.*?)\\n+(ĐỀ KIỂM TRA.*?)\\n+\\s*(Lớp:)", "$1\n\n$2\n\n$3");


        String htmlMarkdown = htmlRenderer.render(markdownParser.parse(reformatMarkdown));

        htmlMarkdown = htmlMarkdown
                .replaceAll("<pre><code>\\s*(Lớp:.*?Thời gian:.*?phút)\\s*", "<div class=\"exam-info\">$1</div>")
                .replaceAll("(Lớp:\\s*\\d+)\\s+(Môn:.*?)\\s+(Thời gian:.*?phút)", "$1<br/>$2<br/>$3")
                .replaceAll("<pre><code>\\s*---\\s*(PHẦN.*?)\\s*---\\s*</code></pre>", "<h3>$1</h3>")
                .replaceAll("(Câu \\d+:.*?)(A\\.)", "$1<br/><br/>$2")
                .replaceAll("(A\\.\\s*.*?)(B\\.)", "$1<br/><br/>$2")
                .replaceAll("(B\\.\\s*.*?)(C\\.)", "$1<br/><br/>$2")
                .replaceAll("(C\\.\\s*.*?)(D\\.)", "$1<br/><br/>$2")
                .replaceAll("(Câu \\d+:.*?trình bày.*?\\.)", "<div class=\"essay-question\">$1</div>");

        Document doc = Jsoup.parseBodyFragment(htmlMarkdown);
        doc.outputSettings().syntax(Document.OutputSettings.Syntax.xml);
        htmlMarkdown = doc.body().html();



            Context context = new Context();
            context.setVariable("contentFromMarkdown", htmlMarkdown);
            String renderHtml = templateEngine.process("examTemplate", context);

            try (ByteArrayOutputStream os = new ByteArrayOutputStream()){
                PdfRendererBuilder builder = new PdfRendererBuilder();
                builder.useFont(() -> getClass().getClassLoader().getResourceAsStream("Fonts/DejaVuSans.ttf"),"DejaVu Sans");
                builder.withHtmlContent(renderHtml, new ClassPathResource("/").getURL().toString());
                builder.toStream(os);
                builder.run();
                byte[] pdfBytes = os.toByteArray();
                String contentType = "application/pdf";
                return new ByteArrayMultipartFile(pdfBytes, "file",key, contentType);
            }catch (Exception e) {
                e.printStackTrace();
                System.err.println("Lỗi khi tải file lên Supabase: " + e.getMessage());
                return null;
        }
    }

    private TopicStructureResponse buildTopicTree(Topic currentTopic, DifficultyLevel difficultLevel) {
        TopicStructureResponse topicStructureResponse = modelMapper.map(currentTopic, TopicStructureResponse.class);
        topicStructureResponse.setQuestionStructureResponses(new ArrayList<>());
        topicStructureResponse.setTopicStructureResponses(new ArrayList<>());

        // 1. Thêm các câu hỏi trực tiếp thuộc về chủ đề này (ở mức độ khó yêu cầu)
        List<Question> questions = questionRepo.findAllByTopicId(currentTopic.getId())
                .orElseThrow(() -> new QuestionException("Empty list question for topic " + currentTopic.getId(), HttpStatus.NOT_FOUND))
                .stream()
                .filter(question -> question.getDifficultyLevel().toString().equals(difficultLevel.toString()))
                .toList();

        for (Question question : questions) {
            QuestionStructureResponse questionStructureResponse = modelMapper.map(question, QuestionStructureResponse.class);
            questionStructureResponse.setAnswerStructureResponses(new ArrayList<>());

            List<Answer> answers = answerRepo.findAllByQuestionId(question.getId())
                    .orElseThrow(() -> new AnswerException("Empty list answer for question " + question.getId(), HttpStatus.NOT_FOUND));
            List<AnswerStructureResponse> answerStructureResponses = answers.stream()
                    .map(answer -> modelMapper.map(answer, AnswerStructureResponse.class))
                    .toList();
            questionStructureResponse.setAnswerStructureResponses(answerStructureResponses);
            topicStructureResponse.getQuestionStructureResponses().add(questionStructureResponse);
        }

        // 2. Tìm và thêm các chủ đề con
        // Giả định bạn có một phương thức trong topicRepo để tìm các chủ đề con dựa trên parentId
        List<Topic> subTopics = topicRepo.findAllByParentId(currentTopic.getId()).orElseThrow(()-> new TopicException("No subtopic found",HttpStatus.NOT_FOUND)); // Trả về danh sách rỗng nếu không có

        for (Topic subTopic : subTopics) {
            // Đệ quy gọi lại hàm để xây dựng cây cho chủ đề con
            TopicStructureResponse subTopicResponse = buildTopicTree(subTopic, difficultLevel);
            topicStructureResponse.getTopicStructureResponses().add(subTopicResponse);
        }

        return topicStructureResponse;
    }



}