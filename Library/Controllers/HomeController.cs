using System.Diagnostics;
using System.Security.Claims;
using Library.Data;
using Library.Extensions;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Services;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Library.Controllers;

public class HomeController(IRentalService rentalService, IUserService userService, IBookService bookService) : Controller
{
    
    [Authorize]
    public async Task<IActionResult> Index(string searchString, string sortField, string sortOrder)
    {
        ViewBag.CurrentSearch = searchString;
        ViewBag.CurrentSortField = sortField;
        ViewBag.CurrentOrder = sortOrder;
        
    
        // Verificando se o usuário logado tem aluguel ativo
        var userId = User.GetUserId();
        var userHasBook = await userService.HasRentedBookAsync(userId); 
        var books = await bookService.GetFilteredBooks(searchString,sortOrder,sortField);
        

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