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

    public async Task<IActionResult> Relatorios()
    {
        
        var users = await userService.GetActiveUsersAsync();
        //Lendo os dados para o viewmodel
        var rent = await rentalService.GetActiveRents();
        var returnsRequests = await requestService.GetActiveRequestByType(RequestTypeEnum.RETURNS);
        var signupRequests = await requestService.GetActiveRequestByType(RequestTypeEnum.REGISTER);
        
        //Convertendo para DTO
        var returnsRequestsDto = returnsRequests.Select(async r => new ReturnRequestDTO()
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
        
        //Criando o ViewModel
        var viewModel = new AdminReportViewModel()
        {
            Books = await bookService.GetAllBooksAsync(),
            Rents = rent,
            Users = users,
            SignUpRequests =  await Task.WhenAll(signupRequestsDto),
            ReturnsRequests = await Task.WhenAll(returnsRequestsDto),
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
}