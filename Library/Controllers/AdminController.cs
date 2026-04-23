using Library.Models;
using Library.Models.DTO;
using Library.Models.Enums;
using Library.Models.ViewModel;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(
    ILogger<AdminController> logger,
    IUserService userService,
    IRentalService rentalService,
    IBookService bookService,
    IRequestService requestService
    ) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Admin"))
            return RedirectToAction("Index", "Access");
        var signupRequests = await requestService.GetActiveRequestByType(RequestTypeEnum.REGISTER);
        
        var signupRequestsDto = signupRequests.Select(async r => new RegisterRequestDTO()
        {
            Id = r.Id,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
            Status = r.Status,
            Type = r.Type,
            User = r.User,
            UserId = r.UserId,
            Body = await requestService.ResolveRequestBody(r) as RegisterRequestBody
        });
        
        var viewModel = new AdminDashboardViewModel
        {
            // Busca solicitações pendentes
            PendingRequests = await Task.WhenAll(signupRequestsDto),
            // Busca aluguéis sem data de retorno
            ActiveRents = await rentalService.GetActiveRents(5),
            TotalActiveRents = await rentalService.GetActiveRentsCountAsync(),
            TotalPendingRequests = await requestService.GetPendingRequestsCountAsync(RequestTypeEnum.REGISTER),
            // Métrica de Inventário
            TotalBooks = await bookService.CountAsync(),
            AvailableBooks = await bookService.CountAvailableAsync()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await requestService.ApproveAsync(id);
        if (!result.success)
        {
            ViewData["ErrorMessage"] = result.message;
        }
        return RedirectToAction("Relatorios", "Admin");
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
            TempData["SuccessMessage"] = $"O livro {book.Title} de {book.Author} foi guardado nas estantes";
            return RedirectToAction("RegisterBook", "Admin");
        }
        // SE CHEGOU AQUI, O MODEL ESTÁ INVÁLIDO. 
        ViewBag.Authors = bookService.GetAuthorsAsync();
        
        return View(book);
    }
    
    [HttpPost]
    public async Task<IActionResult> ResetPassword(Guid id, string password = "")
    {
        var result = await userService.ResetPasswordAsync(id, password);
        if (!result.success)
        {
            TempData["ErrorMessage"] = result.message;
        }
        TempData["SuccessMessage"] = "Senha alterada com sucesso!";
        return RedirectToAction("Relatorios", "Admin");
        
    }

    public async Task<IActionResult> Relatorios()
    {
        var users = await userService.GetActiveUsersAsync();
        var rent = await rentalService.GetActiveRents();
        var returnsRequests = await requestService.GetActiveRequestByType(RequestTypeEnum.RETURNS);
        var signupRequests = await requestService.GetActiveRequestByType(RequestTypeEnum.REGISTER);
    
        // Processando Devoluções de forma sequencial
        var returnsRequestsDto = new List<ReturnRequestDTO>();
        foreach (var r in returnsRequests)
        {
            returnsRequestsDto.Add(new ReturnRequestDTO()
            {
                Id = r.Id,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                Status = r.Status,
                Type = r.Type,
                User = r.User,
                UserId = r.UserId,
                Body = await requestService.ResolveRequestBody(r) as ReturnsRequestBody
            });
        }

        // Processando Cadastros de forma sequencial
        var signupRequestsDto = new List<RegisterRequestDTO>();
        foreach (var r in signupRequests)
        {
            signupRequestsDto.Add(new RegisterRequestDTO()
            {
                Id = r.Id,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                Status = r.Status,
                Type = r.Type,
                User = r.User,
                UserId = r.UserId,
                Body = await requestService.ResolveRequestBody(r) as RegisterRequestBody
            });
        }
    
        var viewModel = new AdminReportViewModel()
        {
            Books = await bookService.GetAllBooksAsync(),
            Rents = rent,
            Users = users,
            SignUpRequests = signupRequestsDto,
            ReturnsRequests = returnsRequestsDto,
        };
        return View("Relatorios/Relatorios", viewModel);
    }
    
    public IActionResult GetUsers()
    {
        
        return PartialView("Relatorios/_PartialUsers");
    }
    
    public IActionResult GetBooks()
    {
        return PartialView("Relatorios/_PartialBooks");
    }

    public IActionResult GetRents()
    {
        return PartialView("Relatorios/_PartialRents");
    }
    
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var result = await bookService.Delete(id);
        if (!result.success)
        {
            TempData["ErrorMessage"] = result.message;
        }
        return RedirectToAction("Relatorios", "Admin");
    }

    public async Task<IActionResult> ForceReturnBook(Guid bookId)
    {
        var result = await rentalService.ReturnBookAsync(bookId);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("Relatorios", "Admin");

        }
        TempData["SuccessMessage"] = "Livro Devolvido com sucesso!";
        return RedirectToAction("Relatorios", "Admin");

    }
    
}