using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Library.Models.ViewModel;
using Library.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Library.Controllers;

public class AccessController(LibraryContext context) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            if(User.FindFirst("IsAdmin")?.Value == "True")
                return RedirectToAction("Index", "Admin");
            if(User.FindFirst("IsAdmin")?.Value == "False")
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
        var user = await context.Clients
            .FirstOrDefaultAsync(c => c.Email == model.Email && c.Password == model.Password);

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
            if(user.IsAdmin) return RedirectToAction("Index", "Admin");
            
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
    public async Task<IActionResult> Register(Client client)
    {
        if (ModelState.IsValid)
        {
            // O GUID e os booleanos padrão já estão definidos no seu Model 
            context.Add(client);
            await context.SaveChangesAsync();
            return RedirectToAction("Pending", "Access");
        }
        return View(client);
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