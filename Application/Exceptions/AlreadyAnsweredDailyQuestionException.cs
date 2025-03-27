namespace BackendBaseTemplate.application.Exceptions;

public class AlreadyAnsweredDailyQuestionException : Exception
{

    public AlreadyAnsweredDailyQuestionException()
        : base("The daily question was already answered today!")
    {
    }
}
