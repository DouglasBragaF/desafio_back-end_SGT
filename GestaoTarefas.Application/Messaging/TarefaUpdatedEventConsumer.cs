using GestaoTarefas.Application.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace GestaoTarefas.Application.Messaging
{
  public class TarefaUpdatedEventConsumer : IConsumer<TarefaUpdatedEvent>
  {
    private readonly IHubContext<TarefasHub> _hubContext;
    public TarefaUpdatedEventConsumer(IHubContext<TarefasHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<TarefaUpdatedEvent> context)
    {
      var message = context.Message;
      Console.WriteLine($"Tarefa atualizada: {message.TarefaId}");
      await _hubContext.Clients.All.SendAsync("TarefaAtualizada", message.TarefaId, message.Tarefa);
    }
  }
}
