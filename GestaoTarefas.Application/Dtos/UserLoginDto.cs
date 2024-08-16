using System.ComponentModel.DataAnnotations;

namespace GestaoTarefas.Application.Dtos
{
  public class UserLoginDto
  {
    [Required(ErrorMessage = "O campo {0} é obrigatório - User")]
    public required string User { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório - Password")]
    public required string Password { get; set; }
  }
}