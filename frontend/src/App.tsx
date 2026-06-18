import { Route, Routes, useNavigate } from "react-router-dom";
import "./App.css";
import QuizPage from "./pages/Quiz/QuizPage";
import ResultPage from "./pages/Result/ResultPage";
import LoginPage from "./pages/Login/LoginPage";
import RegisterPage from "./pages/Register/RegisterPage";
import ProtectedRoute from "./routes/ProtectedRoute";
import ProfilePage from "./pages/Profile/ProfilePage";
import { useAuth } from "./hooks/useAuth";

function App() {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const handleLogout = () => {
    logout();
    navigate("/profile");
  };
  return (
    <div className="app-shell">
      <header className="app-header">
        <span className="app-logo">
          <span className="app-logo-spark">A</span>ce
          <span className="app-logo-spark">I</span>t
        </span>
        <button onClick={handleLogout}>Logout</button>
      </header>
      <main className="app-main">
        <Routes>
          <Route path="/" element={<QuizPage />} />
          <Route path="/results" element={<ResultPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route
            path="/profile"
            element={
              <ProtectedRoute>
                <ProfilePage />
              </ProtectedRoute>
            }
          />
        </Routes>
      </main>
    </div>
  );
}

export default App;
