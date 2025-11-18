using GestaoObras.Data;
using GestaoObras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoObras.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de pagamentos associados a obras:
    /// criação e consulta via API (AJAX).
    /// </summary>
    public class PagamentosController : Controller
    {
        private readonly AppDb _context;

        /// <summary>
        /// Injeta o contexto da base de dados (EF Core).
        /// </summary>
        public PagamentosController(AppDb context)
        {
            _context = context;
        }

        /// <summary>
        /// POST: Pagamentos/Create
        /// Regista um novo pagamento numa obra.
        /// </summary>
        /// <param name="Id_Obra">Id da obra associada.</param>
        /// <param name="NomePessoa">Nome do pagador.</param>
        /// <param name="Valor">Valor do pagamento.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Id_Obra, string NomePessoa, decimal Valor)
        {
            // Validação do nome
            if (string.IsNullOrWhiteSpace(NomePessoa))
            {
                TempData["Error"] = "O nome da pessoa é obrigatório.";
                return RedirectToAction("Details", "Obras", new { id = Id_Obra });
            }

            // Validação básica do valor
            if (Valor <= 0)
            {
                TempData["Error"] = "O valor deve ser maior que zero.";
                return RedirectToAction("Details", "Obras", new { id = Id_Obra });
            }

            var pagamento = new Pagamento
            {
                Id_Obra = Id_Obra,
                Nome = NomePessoa,
                Valor = Valor,
                Data = DateTime.Now
            };

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Pagamento registado com sucesso!";
            return RedirectToAction("Details", "Obras", new { id = Id_Obra });
        }

        /// <summary>
        /// GET: Pagamentos/GetByObra
        /// API (AJAX) que devolve os pagamentos de uma obra.
        /// </summary>
        /// <param name="obraId">Id da obra.</param>
        [HttpGet]
        public async Task<IActionResult> GetByObra(int obraId)
        {
            var pagamentos = await _context.Pagamentos
                .Where(p => p.Id_Obra == obraId)
                .OrderByDescending(p => p.Data)
                .AsNoTracking()
                .Select(p => new
                {
                    nomePessoa = p.Nome,
                    valor = p.Valor,
                    dataRegisto = p.Data.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();

            return Json(pagamentos);
        }
    }
}
