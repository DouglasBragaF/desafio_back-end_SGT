namespace GestaoTarefas.Application.Events
{

  public class TarefaCreatedEvent
  {
    public int TarefaId { get; set; }
    public required CreateTarefaDto Tarefa { get; set; }
  }

  public class TarefaUpdatedEvent
  {
    public int TarefaId { get; set; }
    public required UpdateTarefaDto Tarefa { get; set; }
  }

  public class TarefaDeletedEvent
  {
    public int TarefaId { get; set; }
    public DateTime DataExclusao { get; set; }
  }

  public class TarefaCompletedEvent
  {
    public int TarefaId { get; set; }
    public DateTime DataConclusao { get; set; }
  }
}