using System.Security.Claims;

namespace Library.Extensions;

public static class ClaimsPrincipalExtensions
{
    // Este método permite que você faça User.GetUserId() em qualquer lugar
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        // Busca a claim que contém o ID que salvamos no Login
        var claim = user.FindFirst(ClaimTypes.NameIdentifier);
        
        if (claim == null || string.IsNullOrEmpty(claim.Value))
        {
            return Guid.Empty;
        }

        return Guid.Parse(claim.Value);
    }

    public static string GetHomePage(this ClaimsPrincipal user)
    {
        var isAdmin = user.FindFirst("IsAdmin")?.Value;
        if (isAdmin == "True")
        {
            return "Admin";
        }
        else if (isAdmin == "False")
        {
            return "Home";
        }
        return "Access";
    }
}