using Microsoft.Extensions.DependencyInjection;
using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Data.Models;

namespace Notadesigner.Approximato.Data;

public class TransitionStorageService(IServiceScopeFactory scopeFactory)
{
    private int _pomodoroId;

    private int _sequenceNumber;

    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task AddPomodoroAsync(DateTime startedAt, TimerState destination)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApproximatoDbContext>();

        /// Create a new pomodoro
        var pomodoro = new Pomodoro() { StartedAt = startedAt };
        _sequenceNumber = 0;
        await dbContext.AddAsync(pomodoro);

        var transitionEntity = new Transition()
        {
            Duration = TimeSpan.Zero,
            Pomodoro = pomodoro,
            SequenceNumber = _sequenceNumber,
            State = destination.ToString()
        };
        await dbContext.AddAsync(transitionEntity);

        await dbContext.SaveChangesAsync();

        _pomodoroId = pomodoro.Id;
    }

    public async Task AddTransitionAsync(DateTime startedAt, TimerState destination)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApproximatoDbContext>();

        var now = DateTime.UtcNow;
        var duration = now - startedAt;
        var transitionEntity = new Transition()
        {
            Duration = duration,
            PomodoroId = _pomodoroId,
            SequenceNumber = ++_sequenceNumber,
            State = destination.ToString()
        };
        await dbContext.AddAsync(transitionEntity);

        await dbContext.SaveChangesAsync();
    }
}