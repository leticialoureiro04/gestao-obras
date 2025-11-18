using System.ComponentModel.DataAnnotations;

namespace GestaoObras.Models;

public class Movimento
{
    [Key] public int Id_Movimento { get; set; }
    public string Operacao { get; set; } = "ADD"; // "ADD" | "REMOVE"
    public int Quantidade { get; set; }
    public DateTime Data_Operacao { get; set; } = DateTime.UtcNow;

    public int? Id_Obra { get; set; }
    public Obra? Obra { get; set; }

    public int? Id_Material { get; set; }
    public Material? Material { get; set; }
}
