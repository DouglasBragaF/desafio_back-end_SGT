using System.ComponentModel.DataAnnotations;
using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Application
{
  public class CreateTarefaDto
  {
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(255, ErrorMessage = "O título deve ter no máximo 255 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
    [DataType(DataType.Date, ErrorMessage = "Data de vencimento inválida.")]
    [CustomValidation(typeof(CreateTarefaDto), "ValidateDataVencimento", ErrorMessage = "A data de vencimento não pode ser anterior à data atual.")]
    public DateTime? DataVencimento { get; set; }

    [Required(ErrorMessage = "O status é obrigatório.")]
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;

    public static implicit operator Tarefa(CreateTarefaDto dto)
    {
      return new Tarefa
      {
        Titulo = dto.Titulo,
        Descricao = dto.Descricao,
        // DataVencimento considerada apenas até o dia
        DataVencimento = dto.DataVencimento.HasValue ? dto.DataVencimento.Value.Date : (DateTime?)null,
        Status = dto.Status,
        // DataCriacao como apenas a data, sem horas e minutos
        DataCriacao = DateTime.UtcNow.Date
      };
    }

    // Método de validação customizada para DataVencimento
    public static ValidationResult? ValidateDataVencimento(DateTime? dataVencimento, ValidationContext context)
    {
      if (dataVencimento.HasValue && dataVencimento.Value.Date < DateTime.UtcNow.Date)
      {
        return new ValidationResult("A data de vencimento não pode ser anterior à data atual.");
      }

      return ValidationResult.Success;
    }
  }
}
