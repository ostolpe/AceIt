import { Link, useNavigate } from "react-router-dom";
import Button from "../../components/Button";
import "./DashboardPage.css";

const topics = ["C#", ".NET", "OOP", "LINQ", "Collections", "Error Handling", "Testing"];

const DashboardPage = () => {
  const navigate = useNavigate();

  return (
    <div className="dashboard">
      <section className="dashboard-hero">
        <div className="dashboard-eyebrow">Practice session</div>
        <h1 className="dashboard-title">
          Ready to ace your
          <br />
          <span className="dashboard-title-accent">next interview?</span>
        </h1>
        <p className="dashboard-sub">
          A fresh set of questions across core .NET topics, scored instantly by AI.
        </p>
        <div className="dashboard-ctas">
          <Button className="dashboard-start" onClick={() => navigate("/quiz")}>
            Start a quiz →
          </Button>
          <Link to="/profile" className="btn btn-ghost">
            View your progress
          </Link>
        </div>
        <div className="dashboard-topics">
          {topics.map((t) => (
            <span key={t} className="dashboard-topic-chip">
              {t}
            </span>
          ))}
        </div>
      </section>
    </div>
  );
};

export default DashboardPage;
