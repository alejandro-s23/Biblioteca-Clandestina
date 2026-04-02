using Library.Data;
using Library.Extensions;
using Library.Models;
using Library.Models.ViewModel;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers;

[Authorize(Roles = "Admin")] // Futuramente você filtrará por IsAdmin
public class AdminController(LibraryContext context, IUserService userService) : Controller
{
    private readonly LibraryContext _context = context;
    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Admin"))
            return RedirectToAction("Index", "Access");
        
        var viewModel = new AdminDashboardViewModel
        {
            // Busca solicitações pendentes
            PendingRequests = await _context.Clients
                .Where(c => !c.IsApproved)
                .OrderBy(c => c.FirstName)
                .Take(5)
                .ToListAsync(),

            // Busca aluguéis sem data de retorno
            ActiveRents = await _context.BookRents
                .Include(br => br.Book)
                .Include(br => br.Client)
                .Where(br => br.ReturnDate == null)
                .Take(5)
                .ToListAsync(),

            // Métrica de Inventário
            TotalBooks = await _context.Books.CountAsync(),
            AvailableBooks = await _context.Books.CountAsync(b => b.Avaliable)
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Approve(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            client?.IsApproved = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index","Admin");
    }

    [HttpGet]
    public async Task<IActionResult> RegisterBook()
    {
        var existingAuthors = await _context.Books
            .Select(b => b.Author)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();

        ViewBag.Authors = existingAuthors;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterBook(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            ViewBag.BookAddSucess = "Livro adicionado com sucesso!";
            return RedirectToAction("RegisterBook", "Admin");
        }
        
        // SE CHEGOU AQUI, O MODEL ESTÁ INVÁLIDO. 
        // PRECISAMOS REABASTECER A LISTA DE AUTORES! [cite: 2025-10-29]
        ViewBag.Authors = await _context.Books
            .Select(b => b.Author)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        
        return View(book);
    }
}