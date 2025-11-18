using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoObras.Models;

public class Material
{
   [Key] public int Id_Material { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public int Stock_Disponivel { get; set; }

    public ICollection<Movimento> Movimentos { get; set; } = new List<Movimento>();
}
