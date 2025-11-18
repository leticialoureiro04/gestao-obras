using GestaoObras.Data;
using GestaoObras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoObras.Controllers;

/// <summary>
/// Controller responsável pela gestão de Clientes:
/// listagem, detalhes, criação, edição e eliminação.
/// </summary>
public class ClientesController : Controller
{
    private readonly AppDb _ctx;

    /// <summary>
    /// Injeta o contexto da base de dados (EF Core).
    /// </summary>
    /// <param name="ctx">Instância de AppDb (DbContext da aplicação).</param>
    public ClientesController(AppDb ctx)
    {
        _ctx = ctx;
    }

    /// <summary>
    /// GET: /Clientes
    /// Lista todos os clientes.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        // AsNoTracking para leituras mais rápidas 
        var clientes = await _ctx.Clientes
            .AsNoTracking()
            .ToListAsync();

        return View(clientes);
    }

    /// <summary>
    /// GET: /Clientes/Details/5
    /// Mostra os detalhes de um cliente.
    /// </summary>
    /// <param name="id">Id do cliente.</param>
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var cliente = await _ctx.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    /// <summary>
    /// GET: /Clientes/Create
    /// Apresenta o formulário para criar um novo cliente.
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// POST: /Clientes/Create
    /// Recebe os dados do formulário e grava um novo cliente.
    /// </summary>
    /// <param name="model">Modelo vindo do formulário.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente model)
    {
        // Se houver erro de validação, volta à view com as mensagens de erro
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _ctx.Clientes.Add(model);
        await _ctx.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// GET: /Clientes/Edit/5
    /// Mostra o formulário para editar um cliente existente.
    /// </summary>
    /// <param name="id">Id do cliente.</param>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var cliente = await _ctx.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    /// <summary>
    /// POST: /Clientes/Edit/5
    /// Atualiza os dados de um cliente existente.
    /// </summary>
    /// <param name="id">Id do cliente (route).</param>
    /// <param name="model">Modelo vindo do formulário.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente model)
    {
        // Segurança extra: garante que o id da rota corresponde ao do modelo
        if (id != model.Id_Cliente)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _ctx.Update(model);
        await _ctx.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// GET: /Clientes/Delete/5
    /// Mostra a página de confirmação para apagar um cliente.
    /// </summary>
    /// <param name="id">Id do cliente.</param>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var cliente = await _ctx.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    /// <summary>
    /// POST: /Clientes/Delete/5
    /// Confirma e executa a eliminação do cliente.
    /// </summary>
    /// <param name="id">Id do cliente.</param>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _ctx.Clientes.FindAsync(id);

        if (cliente is not null)
        {
            _ctx.Clientes.Remove(cliente);
            await _ctx.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
