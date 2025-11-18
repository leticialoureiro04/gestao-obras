using GestaoObras.Data;
using GestaoObras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoObras.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão dos movimentos de stock:
    /// listagem geral, criação de movimentos (saída de stock para obras)
    /// e API para consulta via AJAX.
    /// </summary>
    public class MovimentosController : Controller
    {
        private readonly AppDb _context;

        /// <summary>
        /// Injeta o contexto da base de dados.
        /// </summary>
        public MovimentosController(AppDb context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: /Movimentos
        /// Lista todos os movimentos de stock do sistema.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var movimentos = await _context.Movimentos
                .Include(m => m.Obra)
                    .ThenInclude(o => o.Cliente)
                .Include(m => m.Material)
                .OrderByDescending(m => m.Data_Operacao)
                .AsNoTracking()
                .ToListAsync();

            return View(movimentos);
        }

        /// <summary>
        /// POST: Movimentos/Create
        /// Regista um movimento de saída de material (REMOVE) para uma obra.
        /// </summary>
        /// <param name="Id_Obra">Id da obra onde o material será usado.</param>
        /// <param name="Id_Material">Id do material.</param>
        /// <param name="Quantidade">Quantidade removida do stock.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Id_Obra, int Id_Material, decimal Quantidade)
        {
            // Validação mínima
            if (Quantidade <= 0)
            {
                TempData["Error"] = "A quantidade deve ser maior que zero.";
                return RedirectToAction("Details", "Obras", new { id = Id_Obra });
            }

            // Verificar se o material existe
            var material = await _context.Materiais.FindAsync(Id_Material);
            if (material is null)
            {
                TempData["Error"] = "Material não encontrado.";
                return RedirectToAction("Details", "Obras", new { id = Id_Obra });
            }

            // Verificar stock disponível
            if (material.Stock_Disponivel < Quantidade)
            {
                TempData["Error"] =
                    $"Stock insuficiente. Disponível: {material.Stock_Disponivel}";
                return RedirectToAction("Details", "Obras", new { id = Id_Obra });
            }

            // Criar o movimento REMOVE
            var movimento = new Movimento
            {
                Id_Obra = Id_Obra,
                Id_Material = Id_Material,
                Operacao = "REMOVE",
                Quantidade = (int)Quantidade,
                Data_Operacao = DateTime.Now
            };

            // Atualizar stock
            material.Stock_Disponivel -= (int)Quantidade;

            _context.Movimentos.Add(movimento);
            _context.Materiais.Update(material);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Material registado com sucesso!";
            return RedirectToAction("Details", "Obras", new { id = Id_Obra });
        }

        /// <summary>
        /// GET: Movimentos/GetByObra
        /// API AJAX - lista os movimentos apenas de uma obra.
        /// </summary>
        /// <param name="obraId">Id da obra.</param>
        [HttpGet]
        public async Task<IActionResult> GetByObra(int obraId)
        {
            var movimentos = await _context.Movimentos
                .Include(m => m.Material)
                .Where(m => m.Id_Obra == obraId)
                .OrderByDescending(m => m.Data_Operacao)
                .Select(m => new
                {
                    material = m.Material.Nome,
                    quantidade = m.Quantidade,
                    dataOperacao = m.Data_Operacao.ToString("dd/MM/yyyy HH:mm")
                })
                .AsNoTracking()
                .ToListAsync();

            return Json(movimentos);
        }
    }
}
