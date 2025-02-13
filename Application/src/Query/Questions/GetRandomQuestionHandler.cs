using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendOlimpiadaIsto.application.Query.Questions;

public class GetRandomQuestionHandler
{
    private readonly IRepository<Question> _questionRepository;
    private readonly IRepository<User> _userRepository;
    private readonly SecretsManager _secretsManager;

    public GetRandomQuestionHandler(
        IRepository<Question> questionRepository,
        IRepository<User> userRepository,
        SecretsManager secretsManager

    )
    {
        _questionRepository = questionRepository;
        _userRepository = userRepository;
        _secretsManager = secretsManager;
    }

    public async Task<Question> HandleAsync(Guid? userId)
    {
        IQueryable<Question> queryable;
        if (userId == null || userId == _secretsManager.DefaultAdminGuid)
        {
            queryable = _questionRepository.GetQueryable();
        }
        else
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"user not found for the given id: {userId}");
            var completedQuestionIds = user.AnsweredQuestions.Where(aq => aq.IsFinished).Select(aq => aq.QuestionId);
            queryable = _questionRepository.GetQueryable().Where(q => !completedQuestionIds.Any(qid => qid == q.Id));
        }


        int count = await queryable.CountAsync();
        if (count == 0)
            throw new NotFoundException($"Cannot find any Questions!");

        Random random = new Random();
        int randomIndex = random.Next(count);

        var randomEntity = await queryable.Skip(randomIndex).FirstOrDefaultAsync();
        if (randomEntity == null)
            throw new NotFoundException($"Cannot find any Questions!");

        return randomEntity;
    }

}