using GestaoTarefas.Application.Events;
using MassTransit;

namespace GestaoTarefas.Application.Messaging
{
  public class TarefaDeletedEventConsumer : IConsumer<TarefaDeletedEvent>
  {
    public async Task Consume(ConsumeContext<TarefaDeletedEvent> context)
    {
      var message = context.Message;
      Console.WriteLine($"Tarefa excluída: {message.TarefaId}, Data de exclusão: {message.DataExclusao}");
      // Adicione a lógica de processamento aqui
    }
  }
}
