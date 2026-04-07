// No topo do arquivo, use o método de extensão que criamos [cite: 2025-10-29]

using System.Runtime.InteropServices.JavaScript;
using Library.Data;
using Library.Extensions;
using Library.Models;
using Library.Models.Enums;
using Library.Models.ViewModel;
using Library.Models.ViewModel.DTO;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class AccountController(IUserService userService, IRentalService rentalService, IRequestService requestService) : Controller
{
    
    public async Task<IActionResult> Profile()
    {
        var userId = User.GetUserId(); // Usando a extensão que criamos
        
        var user = await userService.GetUserByIdAsync(userId);
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
        var currentRent = await rentalService.GetActiveRentAsync(userId);
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

        // Criamos um objeto temporário para o transporte dos dados
        var clientData = new User
        {
            Id = User.GetUserId(),
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
        var activeRent = await rentalService.GetActiveRentAsync(User.GetUserId());
        if (activeRent != null)
        {
            //Criando o dictionary para o body da minha request de devolucao
            var requestBody = new ReturnsRequestBody()
            {
                BookId = bookId,
                RentDate =  activeRent.RentDate,
                RentId = activeRent.Id,
            };
            if (!await requestService.CreateRequestAsync(User.GetUserId(), requestBody))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Request failed");
                Console.ResetColor();
                return RedirectToAction("Index", User.GetHomePage());

            }
        }
        
        return RedirectToAction("Profile");
    }
}