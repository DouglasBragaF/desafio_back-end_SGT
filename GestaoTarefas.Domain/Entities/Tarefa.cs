namespace GestaoTarefas.Domain.Entities
{
  public enum StatusTarefa
  {
    Pendente = 0,
    EmProgresso = 1,
    Concluida = 2
  }

  public class Tarefa
  {
    public int? Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataVencimento { get; set; }
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    public DateTime? DataAlteracao { get; set; }

    public Tarefa()
    {
      DataCriacao = DateTime.UtcNow;
    }

    public bool EstaAtrasada()
    {
      return Status != StatusTarefa.Concluida && DataVencimento < DateTime.UtcNow;
    }
  }
}
