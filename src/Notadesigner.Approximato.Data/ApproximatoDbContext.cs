using Microsoft.EntityFrameworkCore;
using Notadesigner.Approximato.Data.Models;

namespace Notadesigner.Approximato.Data;

public class ApproximatoDbContext(DbContextOptions<ApproximatoDbContext> options) : DbContext(options)
{
    public DbSet<Pomodoro> Pomodoros { get; set; }

    public DbSet<Transition> Transitions { get; set; }
}