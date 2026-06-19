namespace AceIt.DTOs;

public record ProfileDto(
string Email,
int TotalSessions,
int TotalQuestionsAnswered,
double OverallAverage,
List<TopicStat> TopicStats);

public record TopicStat(string Topic, double Average, int Questions);
