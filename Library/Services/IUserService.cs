using Library.Models;

namespace Library.Services;

public interface IUserService
{
    Task<(bool Success, string Message)> UpdateProfileAsync(Client client);
    // Você pode adicionar outros métodos aqui, como alteração de senha futuramente
}