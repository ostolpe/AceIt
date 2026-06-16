import type { QuestionCardProps } from "../types";

const QuestionCard = ({
  question,
  answer,
  onAnswer,
  onPrev,
  onNext,
  onReview,
  isLastQuestion,
}: QuestionCardProps) => {
  return (
    <div>
      <h2>{question.text}</h2>
      <input
        value={answer ?? ""}
        type="text"
        onChange={(e) => onAnswer(e.target.value)}
      />
      <button onClick={onPrev}>Previous</button>
      {isLastQuestion ? (
        <button onClick={onReview}>Review Answers</button>
      ) : (
        <button onClick={onNext}>Next</button>
      )}
    </div>
  );
};

export default QuestionCard;
