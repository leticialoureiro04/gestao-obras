using GestaoObras.Data;
using GestaoObras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GestaoObras.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão das Obras:
    /// listagem, detalhes, criação, edição, eliminação e carregamento de dados auxiliares.
    /// </summary>
    public class ObrasController : Controller
    {
        private readonly AppDb _context;

        /// <summary>
        /// Injeta o contexto da base de dados.
        /// </summary>
        public ObrasController(AppDb context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: /Obras
        /// Lista todas as obras com informação do cliente.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var obras = await _context.Obras
                .Include(o => o.Cliente)
                .AsNoTracking()
                .ToListAsync();

            return View(obras);
        }

        /// <summary>
        /// GET: /Obras/Details/5
        /// Mostra os detalhes de uma obra e carrega dados auxiliares:
        /// movimentos, materiais disponíveis, mão de obra e pagamentos.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();

            var obra = await _context.Obras
                .Include(o => o.Cliente)
                .Include(o => o.Movimentos)
                    .ThenInclude(m => m.Material)
                .Include(o => o.MaoDeObra)
                .Include(o => o.Pagamentos)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id_Obra == id);

            if (obra is null)
                return NotFound();

            // Lista apenas materiais com stock > 0
            var materiais = await _context.Materiais
                .Where(m => m.Stock_Disponivel > 0)
                .OrderBy(m => m.Nome)
                .AsNoTracking()
                .ToListAsync();

            ViewData["Materiais"] = materiais;

            return View(obra);
        }

        /// <summary>
        /// GET: /Obras/Create
        /// Prepara o formulário de criação com dropdown de clientes.
        /// </summary>
        public IActionResult Create()
        {
            ViewBag.Clientes = new SelectList(
                _context.Clientes.AsNoTracking(),
                "Id_Cliente",
                "Nome"
            );

            return View();
        }

        /// <summary>
        /// POST: /Obras/Create
        /// Cria uma nova obra.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Obra obra)
        {
            // Garantir separador decimal 
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = new SelectList(
                    _context.Clientes,
                    "Id_Cliente",
                    "Nome",
                    obra.Id_Cliente
                );

                return View(obra);
            }

            _context.Obras.Add(obra);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /Obras/Edit/5
        /// Abre o formulário para editar uma obra existente.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return NotFound();

            var obra = await _context.Obras.FindAsync(id);

            if (obra is null)
                return NotFound();

            ViewBag.Clientes = new SelectList(
                _context.Clientes,
                "Id_Cliente",
                "Nome",
                obra.Id_Cliente
            );

            return View(obra);
        }

        /// <summary>
        /// POST: /Obras/Edit/5
        /// Atualiza os dados de uma obra.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Obra obra)
        {
            if (id != obra.Id_Obra)
                return NotFound();

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = new SelectList(
                    _context.Clientes,
                    "Id_Cliente",
                    "Nome",
                    obra.Id_Cliente
                );

                return View(obra);
            }

            try
            {
                _context.Update(obra);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Obras.Any(e => e.Id_Obra == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /Obras/Delete/5
        /// Mostra confirmação de eliminação.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var obra = await _context.Obras
                .Include(o => o.Cliente)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id_Obra == id);

            if (obra is null)
                return NotFound();

            return View(obra);
        }

        /// <summary>
        /// POST: /Obras/Delete/5
        /// Elimina a obra definitivamente.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obra = await _context.Obras.FindAsync(id);

            if (obra is not null)
            {
                _context.Obras.Remove(obra);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

