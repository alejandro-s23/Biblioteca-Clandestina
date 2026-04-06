using Library.Models;
using Library.Models.ViewModel;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(IUserService userService, IRentalService rentalService, IBookService bookService) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Admin"))
            return RedirectToAction("Index", "Access");
        
        var viewModel = new AdminDashboardViewModel
        {
            // Busca solicitações pendentes
            PendingRequests = await userService.GetUsersAsync(),

            // Busca aluguéis sem data de retorno
            ActiveRents = await rentalService.GetActiveRents(5),

            // Métrica de Inventário
            TotalBooks = await bookService.CountAsync(),
            AvailableBooks = await bookService.CountAvailableAsync()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Approve(Guid id)
    {
        await userService.ApproveUser(id);
        return RedirectToAction("Index","Admin");
    }
    
    [HttpGet]
    public async Task<IActionResult> RegisterBook()
    {
        ViewBag.Authors = await bookService.GetAuthorsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterBook(Book book)
    {
        if (ModelState.IsValid && await bookService.AddBookAsync(book))
        {
            ViewBag.BookAddSucess = "Livro adicionado com sucesso!";
            return RedirectToAction("RegisterBook", "Admin");
        }
        // SE CHEGOU AQUI, O MODEL ESTÁ INVÁLIDO. 
        ViewBag.Authors = bookService.GetAuthorsAsync();
        
        return View(book);
    }
}