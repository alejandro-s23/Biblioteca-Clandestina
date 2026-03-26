using System.Diagnostics;
using System.Security.Claims;
using Library.Data;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers;

public class HomeController(LibraryContext context, IRentalService rentalService) : Controller
{
    
    private readonly LibraryContext _context = context;
    private readonly IRentalService _rentalSv = rentalService;
    
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.ToListAsync();
    
        // Verificamos se o usuário logado tem aluguel ativo
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool userHasBook = false;

        if (userIdString != null)
        {
            var userId = Guid.Parse(userIdString);
            userHasBook = await _context.BookRents.AnyAsync(br => br.ClientId == userId && br.ReturnDate == null);
        }

        ViewBag.UserHasBook = userHasBook;
        return View(books);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpPost]
    public async Task<IActionResult> Reserve(int bookId)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userIdString != null)
        {
            
            Guid userId = Guid.Parse(userIdString);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("GUID: "  + userId.ToString());
            Console.ResetColor();
            var result = await _rentalSv.RentBookAsync(bookId, userId);
            if (result.Success)
            {
                TempData["MensagemSucesso"] = result.Message;
            }
            else
            {
                TempData["MensagemErro"] = result.Message;
            
            }
            
        }

        return RedirectToAction("Index");
    }
}