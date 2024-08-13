using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Application
{
  public class CreateTarefaDto
  {
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataVencimento { get; set; }
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;

    public static implicit operator Tarefa(CreateTarefaDto dto)
    {
      return new Tarefa
      {
        Titulo = dto.Titulo,
        Descricao = dto.Descricao,
        DataVencimento = dto.DataVencimento,
        Status = dto.Status,
        DataCriacao = DateTime.UtcNow // Definindo a Data de Criação no momento da conversão
      };
    }
  }


}