# PhyGen – AI-powered Physics Exam Generator for High School

## 1. Context – Bối cảnh

- Từ lớp 10 đến lớp 12, ngành giáo dục đang chuyển đổi số để nâng cao chất lượng dạy và học. Tuy nhiên, việc tạo đề thi, đặc biệt là môn Vật lý, vẫn còn thủ công và tốn thời gian.
- Các phương pháp kiểm tra truyền thống thiếu tính đa dạng và chuẩn hóa, gây khó khăn trong việc đánh giá năng lực học sinh.
- **PhyGen** sử dụng AI để tự động tạo câu hỏi Vật lý phù hợp với chương trình giáo dục quốc gia, giúp giáo viên tiết kiệm thời gian.

## 2. Problems / Challenges – Vấn đề và thách thức

### Technical Challenges:
- **Xử lý ngôn ngữ tự nhiên (NLP):** Hiểu và tạo câu hỏi trắc nghiệm hoặc tự luận bằng tiếng Việt.
- **Phân loại câu hỏi:** Theo độ khó (dễ, trung bình, khó) và chương (cơ học, điện, quang học, hạt nhân...).
- **Sinh đề bằng AI:** Đảm bảo câu hỏi mạch lạc, không trùng lặp và phù hợp chương trình học.

### Operational Challenges:
- **Cá nhân hóa:** Giáo viên chọn ma trận đề theo trình độ học sinh.
- **Quy trình phê duyệt:** Giáo viên duyệt câu hỏi do AI tạo trước khi sử dụng chính thức.

## 3. Proposed Solutions – Giải pháp đề xuất

### AI-Based Exam Generator (PhyGen Bot):
- Tự động tạo đề theo chủ đề, độ khó và ma trận đề.
- Hỗ trợ tiếng Việt và tuân thủ chương trình học.
- Cho phép tùy chỉnh đề theo yêu cầu giáo viên.

### Teacher Review Dashboard:
- Giáo viên xem, chỉnh sửa và tái sử dụng câu hỏi AI tạo.

### Secure Access and Role Management:
- Quản lý vai trò người dùng và phê duyệt đề chính thức.

## 4. Main Actors

| Actor              | Description                                                                 |
|-------------------|-----------------------------------------------------------------------------|
| **User**          | - Yêu cầu đề thi theo chủ đề, ma trận<br>- Xem và chỉnh sửa đề được tạo     |
| **AI Generator**  | - Chọn câu hỏi phù hợp từ CSDL<br>- Tạo đề và định dạng đề thi               |
| **Admin**         | - Quản lý vai trò người dùng<br>- Quản lý CSDL câu hỏi<br>- Đảm bảo bảo mật |

## 5. Resources & Links

- [Backend Source Code](https://github.com/GitQunA1/Backend_PhyGen)
- [Frontend Source Code (Web)](https://github.com/PhatPTSE184277/phygen-ai-web)
- [Deployed Web App](https://phygen-ai.vercel.app/)
- [Mobile App Source Code](https://github.com/PhatPTSE184277/phygen-ai-mobile)
- [Figma Design (Admin + Mobile)](https://www.figma.com/design/9lRT7kGpW5CsBdRptl2U5Z/SWD?node-id=0-1&t=w0QFfP31WoRnXvQB-1)
- **System Architecture Overview Diagram:** (Runtime view: API, DB, WebAdmin, Mobile, Firebase Storage, Background Job, Redis)
- **ERD Diagrams:** Conceptual, Logical, and Physical
- **Physical DB in Server with Data:** (Backend)
- **Screen List + Screen Map:** (https://docs.google.com/spreadsheets/d/1eecFyFCY56jhuhFvF8JgiCbmiLwCbPZf8m15iWjUcOU/edit?usp=sharing)
- **Mockup Design Project in Figma:** [Figma Link](https://www.figma.com/design/9lRT7kGpW5CsBdRptl2U5Z/SWD?node-id=0-1&t=w0QFfP31WoRnXvQB-1)




