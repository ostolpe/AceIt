export interface Question {
  id: number;
  text: string;
  topic: string;
}

export interface SessionResponse {
  id: number;
  questions: Question[];
}

export interface Answer {
  questionId: number;
  answer: string;
}

export interface FinishRequest {
  id: number;
  answers: Answer[];
}

export interface Result {
  questionId: number;
  score: number;
  feedback: string;
}

export interface SubmitResponse {
  results: Result[];
}

export interface QuestionCardProps {
  question: Question;
  answer: string;
  onAnswer: (answer: string) => void;
  onPrev: () => void;
  onNext: () => void;
  onReview: () => void;
  isLastQuestion: boolean;
  currentIndex: number;
  totalQuestions: number;
}

export interface AnswersReviewProps {
  questions: Question[];
  answers: Answer[];
  onEdit: (questionId: number, answer: string) => void;
  onSubmit: () => void;
}

export interface ResultPageState {
  result: Result[];
  questions: Question[];
  answers: Answer[];
}

export interface AuthContextType {
  token: string | null;
  login: (token: string) => void;
  logout: () => void;
  isAuthenticated: boolean;
}

export interface LoginResponse {
  token: string;
}
