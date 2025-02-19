using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using domain.ValueObjects;

namespace BackendOlimpiadaIsto.application.Query.Users;


public class GetAnsweredQuestionDetailsHandler
{
    private readonly IRepository<User> _userRepository;

    public GetAnsweredQuestionDetailsHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AnsweredQuestion?> HandleAsync(GetAnsweredQuestionDetailsQuery query, Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new ArgumentException($"No user found by id {userId}");

        return user.AnsweredQuestions.FirstOrDefault(aq => aq.QuestionId == query.questionId);
    }
}