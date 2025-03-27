namespace domain.ValueObjects;

public class UserStats
{
    required public int TotalAnsweredQuestions { get; set; }
    required public int MaxStreak { get; set; }
    required public int CurrentStreak { get; set; }
    required public double AvarageQuestionAttempts { get; set; }
}