using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models;

public class Book
{
    public int Id { get; set; }
    [MaxLength(100, ErrorMessage = "O título deve conter no maximo 100 caracteres")]
    [Required(ErrorMessage = "O Campo Título deve ser preenchido")] public string? Title { get; set; }
    [MaxLength(50, ErrorMessage = "O nome do autor deve conter no maximo 50 caracteres")]
    [Required(ErrorMessage = "O Campo Autor deve ser preenchido")] public string? Author { get; set; }
    public bool Avaliable { get; set; } = true;
    public int? IdCurrentRent { get; set; }
    [ForeignKey("IdCurrentRent")]
    public BookRent? CurrentRent { get; set; }

}