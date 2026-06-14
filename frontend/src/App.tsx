import { useEffect, useState } from "react";
import "./App.css";
import type {
  Answer,
  FinishRequest,
  Question,
  Result,
  SessionResponse,
} from "./types";

function App() {
  const baseUrl = "http://localhost:5241";
  const [questions, setQuestions] = useState<Question[]>([]);
  const [answers, setAnswers] = useState<Answer[]>([]);
  const [sessionId, setSessionId] = useState<number>(0);
  const [currIndex, setCurrIndex] = useState<number>(0);
  const [result, setResult] = useState<Result | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    const fetchQuestions = async () => {
      setIsLoading(true);
      try {
        const json = await fetch(`${baseUrl}/api/sessions`);
        const data: SessionResponse = await json.json();
        setQuestions(data.questions);
        setSessionId(data.id);
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
      id: sessionId,
      answers: answers,
    };
    try {
      const json = await fetch(`${baseUrl}/api/sessions/submit`, {
        method: "POST",
        body: JSON.stringify(request),
        headers: {
          "Content-Type": "application/json",
        },
      });
      const data = await json.json();
      setResult(data);
    } catch (err) {
      console.log(err);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (result) {
    return <div>{result.aiFeedback}</div>;
  }
  return (
    <>
      <div>
        <h2>{questions[currIndex]?.text}</h2>
        <input
          value={
            answers.find((a) => a.questionId === questions[currIndex]?.id)
              ?.answer ?? ""
          }
          type="text"
          onChange={(e) =>
            handleAnswer(questions[currIndex].id, e.target.value)
          }
        />
        <button onClick={prev}>Previous</button>
        {currIndex === questions.length - 1 ? (
          <button onClick={submitSession}>Submit</button>
        ) : (
          <button onClick={next}>Next</button>
        )}
      </div>
    </>
  );
}

export default App;
