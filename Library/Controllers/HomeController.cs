using System.Diagnostics;
using Library.Extensions;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        ViewBag.UserHasBook = userHasBook;
        
        //Filtra os livros
        var books = await bookService.GetOrderedBooksAsync(searchString, sortOrder, sortField);
        
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
    public async Task<IActionResult> Reserve(Guid bookId)
    {
        var userId = User.GetUserId();
        var result = await rentalService.RentBookAsync(bookId, userId);
        if (result.Success)
        {
            TempData["SucessMessage"] = result.Message;
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
        }

        return RedirectToAction("Index");
    }
    
}