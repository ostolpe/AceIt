import { Link, Route, Routes, useNavigate } from "react-router-dom";
import "./App.css";
import QuizPage from "./pages/Quiz/QuizPage";
import ResultPage from "./pages/Result/ResultPage";
import AuthPage from "./pages/Auth/AuthPage";
import ProtectedRoute from "./routes/ProtectedRoute";
import ProfilePage from "./pages/Profile/ProfilePage";
import Button from "./components/Button";
import { useAuth } from "./hooks/useAuth";

function App() {
  const { logout, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <div className="app-shell">
      <header className="app-header">
        <Link to="/" className="app-logo">
          <span className="app-logo-spark">A</span>ce
          <span className="app-logo-spark">I</span>t
        </Link>
        <nav className="app-nav">
          {isAuthenticated ? (
            <>
              <Link to="/profile" className="btn btn-ghost">Profile</Link>
              <Button variant="ghost" onClick={handleLogout}>Logout</Button>
            </>
          ) : (
            <>
              <Link to="/login" className="btn btn-ghost">Sign in</Link>
              <Link to="/register" className="btn btn-solid">Get started</Link>
            </>
          )}
        </nav>
      </header>
      <main className="app-main">
        <Routes>
          <Route path="/" element={<QuizPage />} />
          <Route path="/results" element={<ResultPage />} />
          <Route path="/login" element={<AuthPage mode="login" />} />
          <Route path="/register" element={<AuthPage mode="register" />} />
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
