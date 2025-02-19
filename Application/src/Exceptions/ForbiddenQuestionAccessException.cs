namespace BackendOlimpiadaIsto.application.Exceptions;

public class ForbiddenQuestionAccessException : Exception
{

    public ForbiddenQuestionAccessException()
        : base("The question accessed didn't match the daily question!")
    {
    }
}
