using GestaoTarefas.Application.Events;
using MassTransit;

namespace GestaoTarefas.Application.Messaging
{
  public class TarefaCreatedEventConsumer : IConsumer<TarefaCreatedEvent>
  {
    public async Task Consume(ConsumeContext<TarefaCreatedEvent> context)
    {
      var message = context.Message;
      Console.WriteLine($"Tarefa criada: {message.TarefaId}");
    }
  }
}