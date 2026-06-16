import { Route, Routes } from "react-router-dom";
import "./App.css";
import QuizPage from "./pages/Quiz/QuizPage";
import ResultPage from "./pages/Result/ResultPage";

function App() {
  return (
    <div className="app-shell">
      <header className="app-header">
        <span className="app-logo">
          <span className="app-logo-spark">A</span>ce<span className="app-logo-spark">I</span>t
        </span>
      </header>
      <main className="app-main">
        <Routes>
          <Route path="/" element={<QuizPage />} />
          <Route path="/results" element={<ResultPage />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;
