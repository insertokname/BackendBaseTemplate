using System.Linq;
using System.Reflection.Metadata;
using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using domain.ValueObjects;

namespace BackendOlimpiadaIsto.application.Commands.Questions;

public class VerifyQuestionHandler
{
    private readonly IRepository<Question> _questionRepository;
    private readonly IRepository<User> _userRepository;
    private readonly SecretsManager _secretsManager;


    public VerifyQuestionHandler(
        IRepository<Question> repository,
        IRepository<User> userRepository,
        SecretsManager secretsManager
    )
    {
        _questionRepository = repository;
        _userRepository = userRepository;
        _secretsManager = secretsManager;
    }

    public async Task<bool> HandleAsync(VerifyQuestionCommand command, Guid? userId)
    {
        var question = await _questionRepository.GetByIdAsync(command.QuestionId);
        if (question == null)
            throw new NotFoundException($"Question not found for the given id: {command.QuestionId}");
        if (!(0 <= command.GivenAnswerIndex && command.GivenAnswerIndex < question.Answers.Answers.Count()))
        {
            throw new ArgumentException($"Give bad answer index: {command.GivenAnswerIndex}!Out of bounds! (0,{question.Answers.Answers.Count() - 1})");
        }
        bool isCorrect = question.Answers.CorrectAnswerIndex == command.GivenAnswerIndex;


        if (userId != null && userId != _secretsManager.DefaultAdminGuid)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"user not found for the given id: {userId}");
            }

            bool modified = false;
            var answeredQuestion = user.AnsweredQuestions
                .FirstOrDefault(aq => aq.QuestionId == command.QuestionId);
            if (answeredQuestion == null)
            {
                answeredQuestion =
                    new AnsweredQuestion(command.QuestionId, new List<int> { command.GivenAnswerIndex }, isCorrect);
                user.AnsweredQuestions.Add(answeredQuestion);
                modified = true;
            }
            {
                if (answeredQuestion.IsFinished)
                {
                    throw new ArgumentException("Question already answered corectly!");
                }
                    if (!answeredQuestion.Attempts.Any(e => e == command.GivenAnswerIndex))
                    {
                        answeredQuestion.Attempts.Add(command.GivenAnswerIndex);
                        modified = true;
                    }
                    if (answeredQuestion.IsFinished != isCorrect)
                    {
                        answeredQuestion.IsFinished = isCorrect;
                        modified = true;
                    }
            }
            if (modified)
            {
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
            }
        }
        return isCorrect;
    }
}