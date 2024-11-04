using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notadesigner.Approximato.Data.Models;

[Table("Pomodoros")]
public class Pomodoro
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateTime StartedAt { get; set; }

    public List<Transition> Transitions { get; set; } = [];
}