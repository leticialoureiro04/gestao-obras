using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoObras.Models;

public class Obra
{
    [Key] public int Id_Obra { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? Morada { get; set; }
    public decimal? Latitude { get; set; }   // DECIMAL(10,6)
    public decimal? Longitude { get; set; }  // DECIMAL(10,6)
    public bool Ativa { get; set; }

    public int? Id_Cliente { get; set; }
    public Cliente? Cliente { get; set; }

    public ICollection<Movimento> Movimentos { get; set; } = new List<Movimento>();
    public ICollection<MaoDeObra> MaoDeObra { get; set; } = new List<MaoDeObra>();
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}

