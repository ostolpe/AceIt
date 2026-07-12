import { useEffect, useState } from "react";
import type {
  Question,
  Answer,
  Result,
  SessionResponse,
  SubmitResponse,
  FinishRequest,
} from "../../types";
import QuestionCard from "../../components/QuestionCard";
import AnswersReview from "../../components/AnswersReview";
import { Navigate } from "react-router-dom";
import "./QuizPage.css";
import { useApi } from "../../hooks/useApi";

const QuizPage = () => {
  const [questions, setQuestions] = useState<Question[]>([]);
  const [answers, setAnswers] = useState<Answer[]>([]);
  const [sessionId, setSessionId] = useState<number>(0);
  const [currIndex, setCurrIndex] = useState<number>(0);
  const [result, setResult] = useState<Result[] | null>(null);
  const [isReviewing, setIsReviewing] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const { apiFetch } = useApi();

  useEffect(() => {
    const fetchQuestions = async () => {
      setIsLoading(true);
      try {
        const res = await apiFetch<SessionResponse>("/api/sessions");
        setQuestions(res.questions);
        setSessionId(res.id);
      } catch (err) {
        console.log(err);
      } finally {
        setIsLoading(false);
      }
    };
    fetchQuestions();
  }, []);

  const handleAnswer = (questionId: number, answer: string) => {
    setAnswers((prev) => {
      const existing = prev.filter((a) => a.questionId !== questionId);
      return [...existing, { questionId, answer }];
    });
  };

  const prev = () => {
    if (currIndex === 0) return;
    setCurrIndex(currIndex - 1);
  };

  const next = () => {
    if (currIndex === questions.length - 1) return;
    setCurrIndex(currIndex + 1);
  };

  const submitSession = async () => {
    setIsLoading(true);
    const request: FinishRequest = {
      sessionId: sessionId,
      answers: answers,
    };

    try {
      const res = await apiFetch<SubmitResponse>("/api/sessions/submit", {
        method: "POST",
        body: JSON.stringify(request),
      });
      setResult(res.results);
    } catch (err) {
      console.log(err);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading || questions.length === 0) {
    return (
      <div className="loading-screen">
        <span className="loading-dot" />
        <p>Preparing your session…</p>
      </div>
    );
  }

  if (result) {
    return <Navigate to="/results" state={{ result, questions, answers }} />;
  }

  if (isReviewing) {
    return (
      <AnswersReview
        questions={questions}
        answers={answers}
        onEdit={handleAnswer}
        onSubmit={submitSession}
      />
    );
  }
  return (
    <div className="quiz-page">
      <QuestionCard
        question={questions[currIndex]}
        answer={
          answers.find((a) => a.questionId === questions[currIndex]?.id)
            ?.answer ?? ""
        }
        onAnswer={(answer) => handleAnswer(questions[currIndex].id, answer)}
        onPrev={prev}
        onNext={next}
        onReview={() => setIsReviewing(true)}
        isLastQuestion={currIndex === questions.length - 1}
        currentIndex={currIndex}
        totalQuestions={questions.length}
      />
    </div>
  );
};

export default QuizPage;
