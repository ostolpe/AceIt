import { useLocation } from "react-router-dom";
import type { ResultPageState } from "../../types";

const ResultPage = () => {
  const { result, questions, answers }: ResultPageState = useLocation().state;
  const combined = result.map((res) => ({
    result: res,
    answer: answers.find((x) => x.questionId === res.questionId),
    question: questions.find((x) => x.id === res.questionId),
  }));

  const totalScore = combined.reduce((acc, x) => acc + x.result.score, 0);
  const averageScore = Math.round(totalScore / combined.length);
  return (
    <div>
      {combined.map((res) => (
        <div key={res.question.id}>
          <h2>Question: {res.question.text}</h2>
          <p>Your answer: {res.answer.answer}</p>
          <p>Feedback: {res.result.feedback}</p>
          <p>Score: {res.result.score}</p>
        </div>
      ))}
      <h3>Average Score: {averageScore}/10</h3>
    </div>
  );
};

export default ResultPage;
