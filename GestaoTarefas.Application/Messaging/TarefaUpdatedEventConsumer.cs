using GestaoTarefas.Application.Events;
using MassTransit;

namespace GestaoTarefas.Application.Messaging
{
  public class TarefaUpdatedEventConsumer : IConsumer<TarefaUpdatedEvent>
  {
    public async Task Consume(ConsumeContext<TarefaUpdatedEvent> context)
    {
      var message = context.Message;
      Console.WriteLine($"Tarefa atualizada: {message.TarefaId}");
      // Adicione a lógica de processamento aqui
    }
  }
}
