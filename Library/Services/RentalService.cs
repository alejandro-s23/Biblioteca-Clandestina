using Library.Data;
using Library.Data.Interfaces;
using Library.Models;
using Library.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Services;

public class RentalService(
    IBookRepository bookRepository, 
    IBookRentRepository rentRepository, 
    ILogger<RentalService> logger) : IRentalService
{
    public async Task<(bool Success, string Message)> RentBookAsync(Guid bookId, Guid userId)
    {
        
        if (await bookRepository.AnyBookTenantAsync(userId))
        {
            return (false, "Você já possui um manuscrito em sua posse. Devolva-o para retirar um novo.");
        }
        var book = await bookRepository.GetByIdAsync(bookId);
        // 1. Validação de existência e aprovação
        if (book == null) return (false, "O manuscrito não foi encontrado nos registros.");
        
        // 2. Validação de disponibilidade
        if (!book.Available) return (false, "Este manuscrito já está em posse de outro.");

        var newRent = new BookRent
        {
            Id = Guid.NewGuid(),
            BookId = bookId,
            UserId = userId,
            RentDate = DateTime.Now
        };
        
        if(!(await rentRepository.Add(newRent)))
            return (false, "Erro ao inserir novo registro de aluguel.");
        
        book.IdCurrentRent = newRent.Id;
        book.Available = false;

        if (await bookRepository.SaveAsync())
        {
            return (true, "A posse foi selada e registrada.");

        }
        return (false, "Erro inesperado.");
    }

    public async Task<(bool Success, string Message)> ReturnBookAsync(Guid bookId)
    {
        var book = await bookRepository.GetByIdLoadedAsync(bookId);
        if (book?.CurrentRent == null)
            return (false, "Não há registro de posse ativa para este volume.");
        // Encerra o aluguel atual
        book.CurrentRent.ReturnDate = DateTime.Now;
    
        // Limpa a chave estrangeira virtual e libera o livro
        book.IdCurrentRent = null;
        book.Available = true;
        
        if(await bookRepository.Update(book))
        {
            logger.LogInformation($"Livro {book.Title} foi devolvido.");
            return (true, "O manuscrito retornou ao acervo.");

        }
        return (false, "Erro inesperado.");
    }
    
    public async Task<BookRent?> GetActiveRentAsync(Guid userId)
    {
        return await rentRepository.GetActiveRentAsync(userId);
    }

    public async Task<IEnumerable<BookRent>> GetActiveRents(int size = 0)
    {
        return await rentRepository.GetActiveRents(size);
    }

    public async Task<IEnumerable<BookRent>> GetAllRentsAsync()
    {
        var rents = await rentRepository.GetEntitiesAsync();
        var allRentsAsync = rents.ToList();
        var orderedEnumerable = allRentsAsync.OrderByDescending(x => x.RentDate);
        return orderedEnumerable;
    }

    public async Task<int> GetActiveRentsCountAsync()
    {
        return await rentRepository.GetActiveRentsCountAsync();
    }
}