using System.ComponentModel.DataAnnotations;

namespace GestaoObras.Models;

public class Cliente
{
    [Key] public int Id_Cliente { get; set; }
    public string Nome { get; set; } = null!;
    public string? NIF { get; set; }
    public string? Morada { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }

    public ICollection<Obra> Obras { get; set; } = new List<Obra>();
    
}