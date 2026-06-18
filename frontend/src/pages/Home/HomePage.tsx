import { Link } from "react-router-dom";
import "./HomePage.css";

const features = [
  {
    icon: "01",
    title: "Real interview questions",
    body: "Practice with curated questions across 7 core .NET and C# topics — the ones interviewers actually ask.",
  },
  {
    icon: "02",
    title: "Instant AI scoring",
    body: "Every answer gets scored and critiqued immediately. Know exactly what you got right and what you missed.",
  },
  {
    icon: "03",
    title: "Track your weak spots",
    body: "Your profile tracks performance by topic over time. Stop guessing where to focus — let the data show you.",
  },
];

const plans = [
  {
    name: "Starter",
    price: "Free",
    period: "forever",
    annual: null,
    description: "Get a feel for how it works.",
    highlight: false,
    cta: "Get started free",
    href: "/register",
    items: [
      "3 practice sessions / month",
      "5 core topics",
      "AI-scored answers",
      "30-day session history",
    ],
  },
  {
    name: "Pro",
    price: "$8",
    period: "/ month",
    annual: "$6 / mo billed annually",
    description: "For developers serious about landing the role.",
    highlight: true,
    cta: "Start free trial",
    href: "/register",
    items: [
      "Unlimited sessions",
      "All 7 topics",
      "Detailed AI feedback",
      "Full performance history",
      "Topic progress tracking",
      "Priority support",
    ],
  },
  {
    name: "Teams",
    price: "$18",
    period: "/ seat / month",
    annual: null,
    description: "Prep your whole engineering team together.",
    highlight: false,
    cta: "Contact us",
    href: "/register",
    items: [
      "Everything in Pro",
      "Team leaderboard",
      "Custom question sets",
      "Admin dashboard",
      "Bulk seat management",
    ],
  },
];

const topics = ["C#", ".NET", "OOP", "LINQ", "Collections", "Error Handling", "Testing"];

const HomePage = () => {
  return (
    <div className="home">

      {/* Hero */}
      <section className="home-hero">
        <div className="home-hero-inner">
          <div className="home-eyebrow">AI-powered interview prep</div>
          <h1 className="home-headline">
            Stop winging it.<br />
            <span className="home-headline-accent">Start acing it.</span>
          </h1>
          <p className="home-subheadline">
            Practice real .NET interview questions, get scored by AI, and track exactly
            where you need to improve — before the interview.
          </p>
          <div className="home-ctas">
            <Link to="/register" className="btn btn-solid home-cta-primary">
              Get started free
            </Link>
            <Link to="/login" className="btn btn-ghost home-cta-secondary">
              Sign in →
            </Link>
          </div>
          <div className="home-topics">
            {topics.map((t) => (
              <span key={t} className="home-topic-chip">{t}</span>
            ))}
          </div>
        </div>
      </section>

      {/* Stats bar */}
      <div className="home-stats">
        <div className="home-stat">
          <span className="home-stat-value">7</span>
          <span className="home-stat-label">Core topics</span>
        </div>
        <div className="home-stat-divider" />
        <div className="home-stat">
          <span className="home-stat-value">23</span>
          <span className="home-stat-label">Curated questions</span>
        </div>
        <div className="home-stat-divider" />
        <div className="home-stat">
          <span className="home-stat-value">&lt; 2s</span>
          <span className="home-stat-label">AI grading time</span>
        </div>
        <div className="home-stat-divider" />
        <div className="home-stat">
          <span className="home-stat-value">Free</span>
          <span className="home-stat-label">To start</span>
        </div>
      </div>

      {/* How it works */}
      <section className="home-section">
        <div className="home-section-label">How it works</div>
        <h2 className="home-section-title">
          Practice. Get scored. Improve.
        </h2>
        <div className="home-features">
          {features.map((f) => (
            <div key={f.icon} className="home-feature">
              <div className="home-feature-num">{f.icon}</div>
              <h3 className="home-feature-title">{f.title}</h3>
              <p className="home-feature-body">{f.body}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Pricing */}
      <section className="home-section">
        <div className="home-section-label">Pricing</div>
        <h2 className="home-section-title">Start free. Upgrade when ready.</h2>
        <p className="home-section-sub">
          No credit card required to get started.
        </p>
        <div className="home-plans">
          {plans.map((plan) => (
            <div
              key={plan.name}
              className={`home-plan ${plan.highlight ? "home-plan--highlight" : ""}`}
            >
              {plan.highlight && (
                <div className="home-plan-badge">Most popular</div>
              )}
              <div className="home-plan-header">
                <div className="home-plan-name">{plan.name}</div>
                <div className="home-plan-price">
                  <span className="home-plan-amount">{plan.price}</span>
                  <span className="home-plan-period">{plan.period}</span>
                </div>
                {plan.annual && (
                  <div className="home-plan-annual">{plan.annual}</div>
                )}
                <p className="home-plan-desc">{plan.description}</p>
              </div>
              <ul className="home-plan-items">
                {plan.items.map((item) => (
                  <li key={item} className="home-plan-item">
                    <span className="home-plan-check">✓</span>
                    {item}
                  </li>
                ))}
              </ul>
              <Link
                to={plan.href}
                className={`btn home-plan-cta ${plan.highlight ? "btn-solid" : "btn-ghost home-plan-cta--outline"}`}
              >
                {plan.cta}
              </Link>
            </div>
          ))}
        </div>
      </section>

      {/* Bottom CTA */}
      <section className="home-bottom-cta">
        <div className="home-bottom-cta-glow" />
        <h2 className="home-bottom-title">
          Your next interview<br />is closer than you think.
        </h2>
        <p className="home-bottom-sub">
          Join developers who prep smarter, not longer.
        </p>
        <Link to="/register" className="btn btn-solid home-cta-primary">
          Get started — it's free
        </Link>
      </section>

      <footer className="home-footer">
        <span className="app-logo">
          <span className="app-logo-spark">A</span>ce
          <span className="app-logo-spark">I</span>t
        </span>
        <span className="home-footer-copy">© 2026 AceIt. All rights reserved.</span>
      </footer>
    </div>
  );
};

export default HomePage;
