using BackendBaseTemplate.application.Exceptions;
using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure;
using BackendBaseTemplate.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendBaseTemplate.application.Query.Questions;

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

    private async Task<Question> getRandomFromQueryable(IQueryable<Question> queryable)
    {
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

    public async Task<Question> HandleAsync(Guid? userId)
    {
        IQueryable<Question> queryable;
        if (userId == null || userId == _secretsManager.DefaultAdminGuid)
        {
            queryable = _questionRepository.GetQueryable();
            return await getRandomFromQueryable(queryable);
        }
        else
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"user not found for the given id: {userId}");

            if (user.LastAnswerdQuestionStartTime != null
             && user.LastAnswerdQuestionStartTime >= DateTime.UtcNow.Date)
            {
                var question = await _questionRepository.GetByIdAsync(user.LastAnsweredQuestionId!);
                if (question == null)
                {
                    throw new NotFoundException($"Couldn't find a question by the id of: {question}");
                }
                var answeredQuestion = user.AnsweredQuestions.FirstOrDefault(aq => aq.QuestionId == question.Id);
                if (answeredQuestion?.FinishedDate != null)
                {
                    throw new AlreadyAnsweredDailyQuestionException();
                }
                return question;
            }

            var completedQuestionIds = user.AnsweredQuestions.Where(aq => aq.FinishedDate != null).Select(aq => aq.QuestionId);
            queryable = _questionRepository.GetQueryable().Where(q => !completedQuestionIds.Any(qid => qid == q.Id));

            var randomEntity = await getRandomFromQueryable(queryable);

            user.LastAnswerdQuestionStartTime = DateTime.UtcNow.Date;
            user.LastAnsweredQuestionId = randomEntity.Id;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return randomEntity;
        }
    }

}