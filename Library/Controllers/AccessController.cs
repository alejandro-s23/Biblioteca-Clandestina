using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Models.Enums;
using Library.Models.ViewModel;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Library.Controllers;

public class AccessController(
    IUserService userService,
    IRequestService requestService
    ) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            if(User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");
            if(User.IsInRole("User"))
                return RedirectToAction("Index", "Home");
        }
        return View();   
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!ModelState.IsValid) {return View(model);}

        // Busca o cliente nos registros
        var user = await userService.GetUserAsync(model.Email, model.Password);
        if (user != null)
        {
            if (!user.IsApproved)
            {
                return RedirectToAction("Pending", "Access");
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity));
            if(user.IsAdmin) 
                return RedirectToAction("Index", "Admin");
            
            return RedirectToAction("Index","Home");
        }

        ModelState.AddModelError("", "Credenciais não encontradas nos manuscritos antigos.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(User client)
    {
        if (ModelState.IsValid)
        {
            var result = await userService.RegisterAsync(client);
            // O GUID e os booleanos padrão já estão definidos no seu Model 
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.message;
                return View();
            }

            var createSignupRequest = await requestService
                .CreateRequestAsync(client.Id, RequestTypeEnum.REGISTER, new RegisterRequestBody());
            if (!createSignupRequest.success)
            {
                TempData["ErrorMessage"] = createSignupRequest.message;
                return View();
            }
        }
        return RedirectToAction("Pending", "Access");
    }
    
    [HttpGet]
    public IActionResult Pending()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index","Access");
    }
}