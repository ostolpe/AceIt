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
  aiFeedback: string;
}
