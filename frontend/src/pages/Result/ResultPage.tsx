import { useLocation } from "react-router-dom";
import type { ResultPageState } from "../../types";
import "./ResultPage.css";

const ResultPage = () => {
  const { result, questions, answers }: ResultPageState = useLocation().state;
  const combined = result.map((res) => ({
    result: res,
    answer: answers.find((x) => x.questionId === res.questionId),
    question: questions.find((x) => x.id === res.questionId),
  }));

  const totalScore = combined.reduce((acc, x) => acc + x.result.score, 0);
  const averageScore = (totalScore / combined.length).toFixed(1);

  return (
    <div className="result">
      <div className="result-hero">
        <span className="result-label">Your Score</span>
        <div className="result-score">
          <span className="result-score-value">{averageScore}</span>
          <span className="result-score-max">/10</span>
        </div>
      </div>

      <div className="result-list">
        {combined.map((res) => (
          <div className="result-item" key={res.question!.id}>
            <div className="result-item-header">
              <h3 className="result-question">{res.question!.text}</h3>
              <span
                className="result-score-pill"
                style={{ opacity: 0.4 + res.result.score / 10 / 1.67 }}
              >
                {res.result.score}/10
              </span>
            </div>
            <p className="result-answer">
              <span className="result-field-label">Your answer</span>
              {res.answer?.answer || "No answer given"}
            </p>
            <p className="result-feedback">
              <span className="result-field-label">Feedback</span>
              {res.result.feedback}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ResultPage;
