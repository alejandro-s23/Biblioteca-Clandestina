using System.Diagnostics;
using System.Security.Claims;
using Library.Data;
using Library.Extensions;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Library.Controllers;

public class HomeController(LibraryContext context, IRentalService rentalService) : Controller
{
    
    private readonly LibraryContext _context = context;
    private readonly IRentalService _rentalSv = rentalService;
    
    [Authorize]
    public async Task<IActionResult> Index(string searchString, string sortOrder, string sortDirection)
    {
        ViewBag.CurrentSearch = searchString;
        ViewBag.CurrentSort = sortOrder;
        ViewBag.CurrentDirection = sortDirection;
        
        var books = _context.Books.AsQueryable();
    
        // Verificando se o usuário logado tem aluguel ativo
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool userHasBook = false;

        if (userIdString != null)
        {
            var userId = Guid.Parse(userIdString);
            userHasBook = await _context.BookRents.AnyAsync(br => br.ClientId == userId && br.ReturnDate == null);
        }

        if (!string.IsNullOrEmpty(searchString))
        {
            books = books.Where(b => b.Title != null && b.Title.Contains(searchString));
        }
        
        books = sortOrder switch
        {
            "Author" => sortDirection == "desc" ? books.OrderByDescending(b => b.Author) : books.OrderBy(b => b.Author),
            "Available" => sortDirection == "desc" ? books.OrderByDescending(b => b.Avaliable) : books.OrderBy(b => b.Avaliable),
            _ => sortDirection == "desc" ? books.OrderByDescending(b => b.Title) : books.OrderBy(b => b.Title),
        };

        ViewBag.UserHasBook = userHasBook;
        return View(await books.ToListAsync());
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
    public async Task<IActionResult> Reserve(Guid bookId)
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