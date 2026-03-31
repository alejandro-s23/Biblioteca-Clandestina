using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services;

public class UserService(LibraryContext context) : IUserService
{
    public async Task<(bool Success, string Message)> UpdateProfileAsync(Client updatedClient)
    {
        // 1. Localizamos o registro original pelo ID [cite: 2025-10-29]
        var existingClient = await context.Clients.FindAsync(updatedClient.Id);

        if (existingClient == null)
        {
            return (false, "Identidade não localizada nos arquivos.");
        }

        // 2. Mapeamento seletivo (apenas o que o usuário pode mudar) [cite: 2025-10-29]
        
        existingClient.Email = updatedClient.Email;
        existingClient.Phone = updatedClient.Phone;
        existingClient.AddressNumber = updatedClient.AddressNumber;
        existingClient.Address = updatedClient.Address;
        existingClient.District = updatedClient.District;

        // Campos como 'Registration' e 'IsApproved' permanecem intactos! [cite: 2025-10-29]

        try
        {
            await context.SaveChangesAsync();
            return (true, "Seus dados foram atualizados com sucesso.");
        }
        catch (Exception)
        {
            return (false, "Erro ao selar as novas informações no banco de dados.");
        }
    }

    public async Task<bool> IsAdminAsync(Guid id)
    {
        var admin = await context.Clients.Where(c => c.IsAdmin).AnyAsync(c => c.Id == id);
        return admin;
    }
    
}