import type { QuestionCardProps } from "../types";
import "./QuestionCard.css";

const QuestionCard = ({
  question,
  answer,
  onAnswer,
  onPrev,
  onNext,
  onReview,
  isLastQuestion,
  currentIndex,
  totalQuestions,
}: QuestionCardProps) => {
  const progress = ((currentIndex + 1) / totalQuestions) * 100;

  return (
    <div className="question-card">
      <div className="question-meta">
        <span className="question-count">
          {String(currentIndex + 1).padStart(2, "0")} /{" "}
          {String(totalQuestions).padStart(2, "0")}
        </span>
        <span className="question-topic">{question.topic}</span>
      </div>

      <div className="progress-track">
        <div className="progress-fill" style={{ width: `${progress}%` }} />
      </div>

      <h2 className="question-text" key={question.id}>
        {question.text}
      </h2>

      <input
        className="answer-input"
        value={answer ?? ""}
        type="text"
        placeholder="Type your answer…"
        onChange={(e) => onAnswer(e.target.value)}
        autoFocus
      />

      <div className="question-nav">
        <button
          className="btn btn-ghost"
          onClick={onPrev}
          disabled={currentIndex === 0}
        >
          ← Previous
        </button>
        {isLastQuestion ? (
          <button className="btn btn-solid" onClick={onReview}>
            Review Answers →
          </button>
        ) : (
          <button className="btn btn-solid" onClick={onNext}>
            Next →
          </button>
        )}
      </div>
    </div>
  );
};

export default QuestionCard;
