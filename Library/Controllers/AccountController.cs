// No topo do arquivo, use o método de extensão que criamos [cite: 2025-10-29]

using System.Runtime.InteropServices.JavaScript;
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
        if (user == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Client not found");
            Console.ResetColor();
            return RedirectToAction("Index", User.GetHomePage());
        };
        
        var viewModel = new UserProfileViewModel
        {
            FullName = user.FirstName + " " + user.LastName,
            Email = user.Email,
            Registration = user.Registration,
            Cpf = user.CPF,
            Number = user.AddressNumber,
            Address = user.Address,
            District = user.District,
            Phone = user.Phone,
            HomePage = User.GetHomePage()
        };

        // Busca o livro que o usuário está segurando
        var currentRent = await context.BookRents
            .OrderByDescending(x => x.RentDate)
            .Include(bookRent => bookRent.Book)
            .FirstOrDefaultAsync(b => b.ClientId == userId && b.ReturnDate == null);
        if (currentRent != null)
        {
            rentalService.UpdateRentTime(currentRent);
            viewModel.ActiveBook = currentRent.Book;
            viewModel.RentTimeDays = currentRent.RentTimeDays;
            
            return View(viewModel);
        }
        viewModel.ActiveBook = null;
        return View(viewModel);
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
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            District = model.District,
            AddressNumber = model.Number
        };

        var result = await userService.UpdateProfileAsync(clientData);

        if (result.Success) TempData["MensagemSucesso"] = result.Message;
        else TempData["MensagemErro"] = result.Message;

        return RedirectToAction("Index", $"{User.GetHomePage()}");
    }
    
    [HttpPost]
    public async Task<IActionResult> Return(Guid bookId)
    {
        var userId = User.GetUserId();
        var result = await rentalService.ReturnBookAsync(bookId);
        if (result.Success)
        {
            TempData["MenssagemSucesso"] = result.Message;
        }
        else
        {
            TempData["MensagemErro"] = result.Message;
        }
        return RedirectToAction("Profile");
    }
}