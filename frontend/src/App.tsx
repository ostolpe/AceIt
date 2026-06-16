import { Route, Routes } from "react-router-dom";
import "./App.css";
import QuizPage from "./pages/Quiz/QuizPage";
import ResultPage from "./pages/Result/ResultPage";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<QuizPage />} />
        <Route path="/results" element={<ResultPage />} />
      </Routes>
    </>
  );
}

export default App;
