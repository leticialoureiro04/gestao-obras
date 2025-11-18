using GestaoObras.Data;
using GestaoObras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoObras.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de registos de mão de obra
    /// associados a uma obra (registo de horas e consulta via AJAX).
    /// </summary>
    public class MaoDeObraController : Controller
    {
        private readonly AppDb _context;

        /// <summary>
        /// Injeta o contexto da base de dados (EF Core).
        /// </summary>
        /// <param name="context">Instância de AppDb (DbContext da aplicação).</param>
        public MaoDeObraController(AppDb context)
        {
            _context = context;
        }

        /// <summary>
        /// POST: MaoDeObra/Create
        /// Regista um novo lançamento de mão de obra para uma obra.
        /// </summary>
        /// <param name="Id_Obra">Id da obra à qual a mão de obra pertence.</param>
        /// <param name="NomePessoa">Nome da pessoa que realizou o trabalho.</param>
        /// <param name="HorasTrabalhadas">Número de horas trabalhadas.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int Id_Obra,
            string NomePessoa,
            decimal HorasTrabalhadas)
        {
            // Validação básica do nome
            if (string.IsNullOrWhiteSpace(NomePessoa))
            {
                TempData["Error"] = "O nome da pessoa é obrigatório.";
                return RedirectToAction("Details", "Obras", new { id = Id_Obra });
            }

            // Validação básica das horas
            if (HorasTrabalhadas <= 0)
            {
                TempData["Error"] = "As horas trabalhadas devem ser maior que zero.";
                return RedirectToAction("Details", "Obras", new { id = Id_Obra });
            }

            var maoDeObra = new MaoDeObra
            {
                Id_Obra = Id_Obra,
                Nome = NomePessoa,
                Horas = HorasTrabalhadas,
                Data = DateTime.Now
            };

            _context.MaoDeObra.Add(maoDeObra);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Mão de obra registada com sucesso!";
            return RedirectToAction("Details", "Obras", new { id = Id_Obra });
        }

        /// <summary>
        /// GET: MaoDeObra/GetByObra
        /// API (AJAX) para obter a lista de mão de obra de uma obra.
        /// </summary>
        /// <param name="obraId">Id da obra.</param>
        [HttpGet]
        public async Task<IActionResult> GetByObra(int obraId)
        {
            var maoDeObra = await _context.MaoDeObra
                .Where(m => m.Id_Obra == obraId)
                .OrderByDescending(m => m.Data)
                .AsNoTracking()
                .Select(m => new
                {
                    nomePessoa = m.Nome,
                    horasTrabalhadas = m.Horas,
                    dataRegisto = m.Data.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();

            return Json(maoDeObra);
        }
    }
}
