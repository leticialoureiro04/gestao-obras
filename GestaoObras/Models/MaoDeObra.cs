using System.ComponentModel.DataAnnotations;

namespace GestaoObras.Models;

public class MaoDeObra
{
    [Key] public int Id_Mao_Obra { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Horas { get; set; }        // DECIMAL(5,2)
    public DateTime Data { get; set; } = DateTime.UtcNow;

    public int? Id_Obra { get; set; }
    public Obra? Obra { get; set; }
}
