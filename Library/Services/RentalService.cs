using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services;

public class RentalService(LibraryContext context) : IRentalService
{
    public async Task<(bool Success, string Message)> RentBookAsync(Guid bookId, Guid clientId)
    {
        var book = await context.Books.FindAsync(bookId);
        var client = await context.Clients.FindAsync(clientId);
        
        var hasActiveRent = await context.BookRents
            .AnyAsync(br => br.ClientId == clientId && br.ReturnDate == null);

        if (hasActiveRent)
        {
            return (false, "Você já possui um manuscrito em sua posse. Devolva-o para retirar um novo.");
        }
        
        // 1. Validação de existência e aprovação
        if (book == null) return (false, "O manuscrito não foi encontrado nos registros.");
        if (client == null) return (false, "O portador não está identificado.");
        if (!client.IsApproved) return (false, "O acesso deste portador ainda não foi selado (aprovado).");
        
        // 2. Validação de disponibilidade
        if (!book.Avaliable) return (false, "Este manuscrito já está em posse de outro.");

        var newRent = new BookRent
        {
            BookId = bookId,
            ClientId = clientId,
            RentDate = DateTime.Now
        };

        // 3. O SEGREDO: Vinculamos o objeto, não o ID
        // Ao atribuir o objeto inteiro, o EF Core sincroniza os IDs automaticamente
        // após o SaveChangesAsync().
        book.CurrentRent = newRent;
        book.Avaliable = false;

        context.BookRents.Add(newRent);
        // 4. Salvando no banco de Dados
        await context.SaveChangesAsync();

        return (true, "A posse foi selada e registrada.");
    }

    public async Task<(bool Success, string Message)> ReturnBookAsync(Guid bookId)
    {
        var book = await context.Books
            .Include(b => b.CurrentRent)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null || book.CurrentRent == null)
            return (false, "Não há registro de posse ativa para este volume.");

        // Encerra o aluguel atual
        book.CurrentRent.ReturnDate = DateTime.Now;
    
        // Limpa a chave estrangeira virtual e libera o livro
        book.IdCurrentRent = null; 
        book.Avaliable = true;

        await context.SaveChangesAsync();
        return (true, "O manuscrito retornou ao acervo.");
    }

    public async Task<bool> IsBookAvailableAsync(Guid bookId)
    {
        return await context.Books.AnyAsync(b => b.Id == bookId && b.Avaliable);
    }

    public bool UpdateRentTime(BookRent? bookRent)
    {
        if (bookRent == null)
            return false;
        bookRent.RentTimeDays = (DateTime.Now - bookRent.RentDate).Days;
        return true;
    }

    
}