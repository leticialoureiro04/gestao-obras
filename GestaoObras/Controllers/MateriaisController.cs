using GestaoObras.Data;
using GestaoObras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoObras.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão dos Materiais:
    /// listagem, detalhes, criação, edição, eliminação e API auxiliar.
    /// </summary>
    public class MateriaisController : Controller
    {
        private readonly AppDb _ctx;

        /// <summary>
        /// Injeta o contexto da base de dados.
        /// </summary>
        public MateriaisController(AppDb ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// GET: /Materiais
        /// Lista todos os materiais.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var materiais = await _ctx.Materiais
                .AsNoTracking()
                .ToListAsync();

            return View(materiais);
        }

        /// <summary>
        /// GET: /Materiais/Details/5
        /// Mostra os detalhes de um material.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();

            var material = await _ctx.Materiais.FindAsync(id);
            return material is null ? NotFound() : View(material);
        }

        /// <summary>
        /// GET: /Materiais/Create
        /// Apresenta o formulário de criação.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: /Materiais/Create
        /// Cria o material e regista um movimento ADD se houver stock inicial.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1) Guardar o material
            _ctx.Materiais.Add(model);
            await _ctx.SaveChangesAsync(); 

            // 2) Se tiver stock inicial, criar movimento de CREATE (ADD)
            if (model.Stock_Disponivel > 0)
            {
                var movimentoInicial = new Movimento
                {
                    Operacao = "ADD",
                    Quantidade = model.Stock_Disponivel,
                    Data_Operacao = DateTime.UtcNow,
                    Id_Obra = null,                  // Sem obra associada
                    Id_Material = model.Id_Material  // FK do material criado
                };

                _ctx.Movimentos.Add(movimentoInicial);
                await _ctx.SaveChangesAsync();
            }

            TempData["Success"] = "Material criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /Materiais/Edit/5
        /// Mostra formulário de edição.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return NotFound();

            var material = await _ctx.Materiais.FindAsync(id);
            return material is null ? NotFound() : View(material);
        }

        /// <summary>
        /// POST: /Materiais/Edit/5
        /// Atualiza um material.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Material model)
        {
            if (id != model.Id_Material)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            _ctx.Update(model);
            await _ctx.SaveChangesAsync();

            TempData["Success"] = "Material atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /Materiais/Delete/5
        /// Mostra a confirmação de eliminação.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var material = await _ctx.Materiais.FindAsync(id);
            return material is null ? NotFound() : View(material);
        }

        /// <summary>
        /// POST: /Materiais/Delete/5
        /// Elimina o material permanentemente.
        /// </summary>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _ctx.Materiais.FindAsync(id);

            if (material is not null)
            {
                _ctx.Materiais.Remove(material);
                await _ctx.SaveChangesAsync();
            }

            TempData["Success"] = "Material eliminado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: API retorna todos os materiais com stock para dropdowns AJAX.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var materiais = await _ctx.Materiais
                .Where(m => m.Stock_Disponivel > 0)
                .OrderBy(m => m.Nome)
                .AsNoTracking()
                .Select(m => new
                {
                    id_Material = m.Id_Material,
                    nome = m.Nome,
                    stock = m.Stock_Disponivel
                })
                .ToListAsync();

            return Json(materiais);
        }
    }
}


