using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;

namespace BackendOlimpiadaIsto.application.Query.Questions;

public class IsDailyQuestionAvailableHandler
{
    private readonly IRepository<User> _userRepository;

    public IsDailyQuestionAvailableHandler(
        IRepository<User> userRepository
    )
    {
        _userRepository = userRepository;
    }

    public async Task<bool> HandleAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException($"user not found for the given id: {userId}");

        if (user.LastAnswerdQuestionStartTime == null
         || user.LastAnswerdQuestionStartTime < DateTime.UtcNow.Date)
            return true;

        var answeredQuestion = user.AnsweredQuestions.FirstOrDefault(aq => aq.QuestionId == user.LastAnsweredQuestionId);
        if (answeredQuestion?.FinishedDate == null)
            return true;

        return false;
    }

}