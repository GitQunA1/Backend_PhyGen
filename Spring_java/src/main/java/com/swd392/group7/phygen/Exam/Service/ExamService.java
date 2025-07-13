package com.swd392.group7.phygen.Exam.Service;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Account.Service.AccountService;
import com.swd392.group7.phygen.Answer.Dto.AnswerStructureResponse;
import com.swd392.group7.phygen.Answer.Entity.Answer;
import com.swd392.group7.phygen.Answer.Exception.AnswerException;
import com.swd392.group7.phygen.Answer.Repository.AnswerRepo;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Dto.*;
import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.Exam.Exception.ExamException;
import com.swd392.group7.phygen.Exam.Repository.ExamRepo;
import com.swd392.group7.phygen.ExamMatrix.Dto.ExamMatrixStructureResponse;
import com.swd392.group7.phygen.ExamMatrix.ExamMatrix;
import com.swd392.group7.phygen.ExamMatrix.Exception.ExamMatrixException;
import com.swd392.group7.phygen.ExamMatrix.Repository.ExamMatrixRepo;
import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.ExamSection.Exception.ExamSectionException;
import com.swd392.group7.phygen.ExamSection.Repository.ExamSectionRepo;
import com.swd392.group7.phygen.MatrixDetail.Dto.MatrixDetailStructureResponse;
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
import com.swd392.group7.phygen.Topic.Service.TopicService;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

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
    private final TopicService topicService;
    private final QuestionRepo questionRepo;
    private final AnswerRepo answerRepo;

    public Exam findExamById(Long id){
        return examRepo.findById(id).orElseThrow(()-> new ExamException("Cant found that Exam", HttpStatus.NOT_FOUND));
    }

    public Response create(CreateRequest examRq){
        Account account = accountService.getCurrentAccount();
        Exam exam = new Exam();
        exam.setExamType(examRq.getExamType());
        exam.setCreatedAt(exam.getCreatedAt());
        exam.setAccount(account);
        examRepo.save(exam);
        return modelMapper.map(exam, Response.class);
    }

    public Response update(UpdateRequest examRq, Long id){
        Exam oldExam = findExamById(id);
        oldExam.setExamType(examRq.getExamType());
        oldExam.setCreatedAt((examRq.getCreateAt()));
        examRepo.save(oldExam);
        return modelMapper.map(oldExam,Response.class);
    }

    public void deleted(Long id){
        Exam exam = findExamById(id);
        exam.setDraft(true);
        examRepo.save(exam);
    }

    public PagedResponse<Response> getAllExamFromAccountId(
            int page,
            int size,
            String sortBy,
            String sortDir){
        Account account = accountService.getCurrentAccount();
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));

        Page<Exam> examPage = examRepo.findAllByAccountId(account.getId(), pageRequest);
        if(examPage.isEmpty()){
            throw new ExamException("Empty list exam for accountId: "+account.getId(),HttpStatus.NOT_FOUND);
        }
        List<Response> responses = examPage.stream().map(exam -> modelMapper.map(exam, Response.class)).toList();
        return new PagedResponse<>(
                responses,
                page,
                size,
                examPage.getTotalElements(),
                examPage.getTotalPages(),
                examPage.isLast()
        );
    }

    public ExamStructureResponse getExamStructure(ExamStructureRequest request){

        Subject subject = subjectService.findSubjectById(request.getSubjectId());
        SubjectStructureResponse subjectStructureResponse = modelMapper.map(subject, SubjectStructureResponse.class);


        ExamMatrix examMatrix = examMatrixRepo.findBySubjectIdAndExamType(request.getSubjectId(),request.getExamType()).orElseThrow(()-> new ExamMatrixException("Empty list exam matrix",HttpStatus.NOT_FOUND));

        ExamMatrixStructureResponse examMatrixStructureResponse = modelMapper.map(examMatrix, ExamMatrixStructureResponse.class);
        examMatrixStructureResponse.setMatrixSectionStructureResponses(new ArrayList<>());

            List<MatrixSection> matrixSections = matrixSectionRepo.findAllByExamMatrixId(examMatrix.getId()).orElseThrow(() -> new MatrixSectionException("Empty list matrix section", HttpStatus.NOT_FOUND));

            for (MatrixSection matrixSection : matrixSections) {
                MatrixSectionStructureResponse matrixSectionStructureResponse = modelMapper.map(matrixSection, MatrixSectionStructureResponse.class);
                matrixSectionStructureResponse.setMatrixDetailStructureResponseList(new ArrayList<>());

                List<MatrixDetail> matrixDetails = matrixDetailRepo.findAllByMatrixSectionIdAndDifficultyLevel(matrixSection.getId(), request.getDifficultLevel()).orElseThrow(() -> new MatrixDetailException("Empty list matrix detail", HttpStatus.NOT_FOUND));

                for (MatrixDetail matrixDetail : matrixDetails) {
                    MatrixDetailStructureResponse matrixDetailStructureResponse = modelMapper.map(matrixDetail, MatrixDetailStructureResponse.class);

                    Topic topic = topicService.findTopicById(matrixDetail.getTopic().getId());
                    TopicStructureResponse topicStructureResponse = modelMapper.map(topic, TopicStructureResponse.class);
                    topicStructureResponse.setQuestionStructureResponses(new ArrayList<>());

                    List<Question> questions = questionRepo.findAllByTopicId(topic.getId()).orElseThrow(() -> new QuestionException("Empty list question", HttpStatus.NOT_FOUND))
                            .stream()
                            .filter(question -> question.getDifficultyLevel().toString().equals(request.getDifficultLevel().toString()))
                            .toList();

                    for (Question question : questions) {
                        QuestionStructureResponse questionStructureResponse = modelMapper.map(question, QuestionStructureResponse.class);
                        questionStructureResponse.setAnswerStructureResponses(new ArrayList<>());

                        List<Answer> answers = answerRepo.findAllByQuestionId(question.getId()).orElseThrow(()-> new AnswerException("Empty list answer", HttpStatus.NOT_FOUND));
                        List<AnswerStructureResponse> answerStructureResponses = answers.stream()
                                .map(answer -> modelMapper.map(answer, AnswerStructureResponse.class))
                                .toList();
                        questionStructureResponse.setAnswerStructureResponses(answerStructureResponses);
                        topicStructureResponse.getQuestionStructureResponses().add(questionStructureResponse);
                    }
                    matrixDetailStructureResponse.setTopicStructureResponse(topicStructureResponse);
                    matrixSectionStructureResponse.getMatrixDetailStructureResponseList().add(matrixDetailStructureResponse);
                }
                examMatrixStructureResponse.getMatrixSectionStructureResponses().add(matrixSectionStructureResponse);
            }
            subjectStructureResponse.setExamMatrixStructureResponse(examMatrixStructureResponse);
        return new ExamStructureResponse(subjectStructureResponse);
    }
}
