import type { AnswersReviewProps } from "../types";

const AnswersReview = ({
  questions,
  answers,
  onSubmit,
  onEdit,
}: AnswersReviewProps) => {
  return (
    <div>
      {questions.map((question) => (
        <div key={question.id}>
          <h3>{question.text}</h3>
          <input
            type="text"
            value={
              answers.find((a) => a.questionId === question.id)?.answer ?? ""
            }
            onChange={(e) => onEdit(question.id, e.target.value)}
          />
        </div>
      ))}
      <button onClick={onSubmit}>Submit Answers</button>
    </div>
  );
};

export default AnswersReview;
