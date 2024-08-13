using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Application
{
  public class UpdateTarefaDto
  {
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataVencimento { get; set; }
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;

    public static implicit operator Tarefa(UpdateTarefaDto dto)
    {
      return new Tarefa
      {
        Id = dto.Id,
        Titulo = dto.Titulo,
        Descricao = dto.Descricao,
        DataVencimento = dto.DataVencimento,
        Status = dto.Status,
        DataAlteracao = DateTime.UtcNow // Definindo a Data de Alteração no momento da conversão
      };
    }
  }
}