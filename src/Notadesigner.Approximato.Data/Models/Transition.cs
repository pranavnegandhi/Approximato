using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notadesigner.Approximato.Data.Models;

[Table("Transitions")]
public class Transition
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(Pomodoro)), Required]
    public int PomodoroId { get; set; }

    [Required]
    public int SequenceNumber { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string? State { get; set; }

    public Pomodoro? Pomodoro { get; set; }
}