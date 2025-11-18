using System.ComponentModel.DataAnnotations;

namespace GestaoObras.Models;

public class Pagamento
{
    [Key] public int Id_Pagamento { get; set; }
    public string? Nome { get; set; }
    public decimal Valor { get; set; }        // DECIMAL(10,2)
    public DateTime Data { get; set; } = DateTime.UtcNow;

    public int? Id_Obra { get; set; }
    public Obra? Obra { get; set; }
}