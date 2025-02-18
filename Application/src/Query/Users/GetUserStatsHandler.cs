using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using domain.ValueObjects;

namespace BackendOlimpiadaIsto.application.Query.Users;


public class GetUserStatsHandler
{
    private readonly IRepository<User> _userRepository;

    public GetUserStatsHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserStats> HandleAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new ArgumentException($"No user found by id {userId}");

        var finishedDates = user.AnsweredQuestions
                .Where(aq => aq.FinishedDate.HasValue)
                .Select(aq => aq.FinishedDate!.Value.Date)
                .Distinct()
                .OrderBy(date => date)
                .ToList();

        int maxStreak = 0;
        int currentStreak = 0;

        if (finishedDates.Any())
        {
            DateTime today = DateTime.UtcNow.Date;
            DateTime expected = (finishedDates.Last() == today) ? today : today.AddDays(-1);
            int tempStreak = 1;

            for (int i = 1; i < finishedDates.Count; i++)
            {
                if ((finishedDates[i] - finishedDates[i - 1]).Days == 1)
                {
                    tempStreak++;
                }
                else
                {
                    if (tempStreak > maxStreak)
                    {
                        maxStreak = tempStreak;
                    }
                    tempStreak = 1;
                }
            }

            var lastFinishedDay = finishedDates.Last().Date;
            if (lastFinishedDay == DateTime.UtcNow.Date
                || lastFinishedDay == DateTime.UtcNow.Date.AddDays(-1)
            )
            {
                currentStreak = tempStreak;
            }

            if (tempStreak > maxStreak)
                maxStreak = tempStreak;

        }

        return new UserStats
        {
            TotalAnsweredQuestions = user.AnsweredQuestions.Count(),
            CurrentStreak = currentStreak,
            MaxStreak = maxStreak,
            AvarageQuestionAttempts = user.AnsweredQuestions.Any() ?
                user.AnsweredQuestions.Average(aq => aq.Attempts.Count) : 0
        };
    }
}