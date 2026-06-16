import type { AnswersReviewProps } from "../types";
import "./AnswersReview.css";

const AnswersReview = ({
  questions,
  answers,
  onSubmit,
  onEdit,
}: AnswersReviewProps) => {
  return (
    <div className="review">
      <div className="review-header">
        <span className="review-label">Review</span>
        <h2 className="review-title">Check your answers</h2>
      </div>

      <div className="review-list">
        {questions.map((question, i) => (
          <div className="review-item" key={question.id}>
            <span className="review-index">
              {String(i + 1).padStart(2, "0")}
            </span>
            <div className="review-item-body">
              <h3 className="review-question">{question.text}</h3>
              <input
                className="review-input"
                type="text"
                placeholder="No answer given"
                value={
                  answers.find((a) => a.questionId === question.id)
                    ?.answer ?? ""
                }
                onChange={(e) => onEdit(question.id, e.target.value)}
              />
            </div>
          </div>
        ))}
      </div>

      <button className="btn btn-solid btn-submit" onClick={onSubmit}>
        Submit Answers →
      </button>
    </div>
  );
};

export default AnswersReview;
