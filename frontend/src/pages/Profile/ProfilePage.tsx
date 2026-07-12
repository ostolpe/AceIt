import { useEffect, useState } from "react";
import { useApi } from "../../hooks/useApi";
import "./ProfilePage.css";

interface ProfileData {
  email: string;
  totalSessions: number;
  totalQuestionsAnswered: number;
  overallAverage: number;
  topicStats: TopicStat[];
}

interface TopicStat {
  topic: string;
  average: number;
  questions: number;
}

interface ScoreCircleProps {
  score: number;
  max?: number;
  size?: number;
  strokeWidth?: number;
  fontSize?: number;
  subFontSize?: number;
}

const ScoreCircle = ({
  score,
  max = 10,
  size = 72,
  strokeWidth = 6,
  fontSize = 16,
  subFontSize = 10,
}: ScoreCircleProps) => {
  const r = (size - strokeWidth) / 2;
  const circumference = 2 * Math.PI * r;
  const pct = Math.min(Math.max(score / max, 0), 1);
  const offset = circumference * (1 - pct);

  return (
    <div className="score-circle" style={{ width: size, height: size }}>
      <svg width={size} height={size}>
        <circle
          className="score-circle-track"
          cx={size / 2}
          cy={size / 2}
          r={r}
          strokeWidth={strokeWidth}
        />
        <circle
          className="score-circle-fill"
          cx={size / 2}
          cy={size / 2}
          r={r}
          strokeWidth={strokeWidth}
          strokeDasharray={circumference}
          strokeDashoffset={offset}
        />
      </svg>
      <div className="score-circle-label">
        <span className="score-circle-value" style={{ fontSize }}>
          {score % 1 === 0 ? score : score.toFixed(1)}
        </span>
        <span className="score-circle-max" style={{ fontSize: subFontSize }}>
          /{max}
        </span>
      </div>
    </div>
  );
};

const ProfilePage = () => {
  const [profileData, setProfileData] = useState<ProfileData | null>(null);
  const { apiFetch } = useApi();

  useEffect(() => {
    const fetchProfileData = async () => {
      try {
        const res = await apiFetch<ProfileData>("/api/profile");
        setProfileData(res);
      } catch (err) {
        console.log(err);
      }
    };
    fetchProfileData();
  }, [apiFetch]);

  return (
    <div className="profile">
      <div className="profile-header">
        <span className="profile-eyebrow">Your profile</span>
        <span className="profile-email">{profileData?.email ?? "—"}</span>
      </div>

      <div className="profile-stats">
        <div className="profile-stat">
          <span className="profile-stat-value">
            {profileData?.totalSessions ?? "—"}
          </span>
          <span className="profile-stat-label">Sessions</span>
        </div>
        <div className="profile-stat">
          <span className="profile-stat-value">
            {profileData?.totalQuestionsAnswered ?? "—"}
          </span>
          <span className="profile-stat-label">Questions answered</span>
        </div>
        <div className="profile-stat">
          <span className="profile-stat-value">
            {profileData ? profileData.overallAverage.toFixed(1) : "—"}
          </span>
          <span className="profile-stat-label">Overall average</span>
        </div>
      </div>

      {profileData && (
        <>
          <div className="profile-overall">
            <div className="profile-section-label">Overall performance</div>
            <div className="profile-overall-inner">
              <ScoreCircle
                score={profileData.overallAverage}
                size={100}
                strokeWidth={8}
                fontSize={22}
                subFontSize={12}
              />
              <div className="profile-overall-text">
                <h2>Average score</h2>
                <p>
                  Across {profileData.totalQuestionsAnswered} questions in{" "}
                  {profileData.totalSessions} session
                  {profileData.totalSessions !== 1 ? "s" : ""}
                </p>
              </div>
            </div>
          </div>

          <div className="profile-topics">
            <div className="profile-section-label">Performance by topic</div>
            <div className="profile-topics-grid">
              {profileData.topicStats.map((stat) => (
                <div className="profile-topic-card" key={stat.topic}>
                  <ScoreCircle score={stat.average} />
                  <div className="profile-topic-info">
                    <span className="profile-topic-name">{stat.topic}</span>
                    <span className="profile-topic-questions">
                      {stat.questions} question{stat.questions !== 1 ? "s" : ""}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default ProfilePage;
