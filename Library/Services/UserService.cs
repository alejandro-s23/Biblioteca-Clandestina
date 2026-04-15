using Library.Data;
using Library.Data.Interfaces;
using Library.Models;
using Library.Models.ViewModel.DTO;
using Library.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Services;

public class UserService(IBookRepository bookRepository, IUserRepository userRepository) : IUserService
{
    public async Task<(bool Success, string Message)> UpdateProfileAsync(User updatedClient)
    {
        // 1. Localizamos o registro original pelo ID [cite: 2025-10-29]
        var existingClient = await userRepository.GetByIdAsync(updatedClient.Id);

        if (existingClient == null)
        {
            return (false, "Identidade não localizada nos arquivos.");
        }

        // 2. Mapeamento seletivo (apenas o que o usuário pode mudar) [cite: 2025-10-29]
        
        existingClient.Email = updatedClient.Email;
        existingClient.Phone = updatedClient.Phone;
        existingClient.Complement = updatedClient.Complement;
        existingClient.AddressNumber = updatedClient.AddressNumber;
        existingClient.Address = updatedClient.Address;
        existingClient.District = updatedClient.District;

        // Campos como 'Registration' e 'IsApproved' permanecem intactos! [cite: 2025-10-29]

        try
        {
            await userRepository.Update(existingClient);
            return (true, "Seus dados foram atualizados com sucesso.");
        }
        catch (Exception)
        {
            return (false, "Erro ao selar as novas informações no banco de dados.");
        }
    }

    public async Task<(bool success, string message)> HasRentedBookAsync(Guid userId)
    {
        return (await bookRepository.AnyBookTenantAsync(userId), string.Empty); 
    }

    public async Task<User?> GetUserAsync(string email, string password)
    {
        return await userRepository.GetUserAsync(email, password);
    }

    public Task<User?> GetUserByIdAsync(Guid userId)
    {
        return userRepository.GetByIdAsync(userId);
    }

    public async Task<(bool success, string message)> RegisterAsync(User client)
    {
        return (await userRepository.Add(client), string.Empty);
    }

    public async Task<IEnumerable<User>> GetUsersAsync(int size = 0)
    {
        return await userRepository.GetEntitiesAsync();
    }

    public async Task<List<User>> GetUsersListAsync(int size = 0)
    {
        return await userRepository.GetEntitiesListAsync();
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync(int size = 0)
    {
        return await userRepository.GetActiveUsersListAsync(size);
    }
    public async Task<List<User>> GetActiveUsersListAsync(int size = 0)
    {
        return await userRepository.GetActiveUsersListAsync(size);
    }

    public async Task<(bool success, string message)> ApproveUser(Guid id)
    {
        var client = await userRepository.GetByIdAsync(id);
        if (client != null)
        {
            client.IsApproved = true;
            return (await userRepository.Update(client), string.Empty);
        }
        return (false, "Erro inesperado ao aprovar o usuário.");
    }

    public async Task<IEnumerable<User>> GetAOrderedUsersAsync(string searchString, string sortOrder, string sortField)
    {
        var users = await userRepository.GetEntitiesAsync();
        /*
        users = sortField switch
        {
            "Approved" => sortOrder == "desc" ? users.OrderByDescending(b => !b.IsApproved) : users.OrderBy(b => !b.IsApproved),
            "Registration" => sortOrder == "desc" ? users.OrderByDescending(b => b.Registration) : users.OrderBy(b => b.Registration),
            _ => sortOrder == "desc" ? users.OrderByDescending(b => b.FirstName) : users.OrderBy(b => b.FirstName),
        };
        */
        return users;
    }
}