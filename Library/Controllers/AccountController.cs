// No topo do arquivo, use o método de extensão que criamos [cite: 2025-10-29]

using Library.Data;
using Library.Extensions;
using Library.Models;
using Library.Models.ViewModel;
using Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers;

public class AccountController(LibraryContext context,IUserService userService, IRentalService rentalService) : Controller
{
    
    public async Task<IActionResult> Profile()
    {
        var userId = User.GetUserId(); // Usando a extensão que criamos
        
        var user = await context.Clients.FindAsync(userId);
        if (user == null) return NotFound();

        // Busca o livro que o usuário está segurando
        var activeBook = await context.Books.Include(book => book.CurrentRent)
            .FirstOrDefaultAsync(b => b.CurrentRent != null && b.CurrentRent.ClientId == userId && b.CurrentRent.ReturnDate == null);
        rentalService.UpdateRentTime(activeBook?.CurrentRent);
        if (activeBook is { CurrentRent: not null })
        {
            var viewModel = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Registration = user.Registration,
                ActiveBook = activeBook,
                RentTimeDays = activeBook.CurrentRent.RentTimeDays
            };
        

            return View(viewModel);
        }

        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
    {
        if (!ModelState.IsValid) return View("Profile", model);

        // Criamos um objeto temporário para o transporte dos dados [cite: 2025-10-29]
        var clientData = new Client
        {
            Id = User.GetUserId(), // Usando o seu método de extensão! [cite: 2025-10-29]
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            District = model.District
        };

        var result = await userService.UpdateProfileAsync(clientData);

        if (result.Success) TempData["MensagemSucesso"] = result.Message;
        else TempData["MensagemErro"] = result.Message;

        return RedirectToAction("Index", $"{User.GetHomePage()}");
    }
    
    
}