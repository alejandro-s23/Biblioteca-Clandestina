using Library.Data;
using Library.Data.Interfaces;
using Library.Models;
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

    public async Task<bool> HasRentedBookAsync(Guid userId)
    {
        return await bookRepository.AnyBookTenantAsync(userId); 
    }

    public async Task<User?> GetUserAsync(string email, string password)
    {
        return await userRepository.GetUserAsync(email, password);
    }

    public async Task<bool> RegisterAsync(User client)
    {
        return await userRepository.Add(client);
    }

    public async Task<List<User>> GetUsersAsync(int size = 5)
    {
        return await userRepository.GetPendingRegistrationsListAsync(size);
    }

    public async Task<bool> ApproveUser(Guid id)
    {
        var client = await userRepository.GetByIdAsync(id);
        if (client != null)
        {
            client.IsApproved = true;
            return await userRepository.Update(client);
        }
        return false;
    }
}