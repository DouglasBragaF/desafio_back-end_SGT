using GestaoTarefas.Application.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace GestaoTarefas.Application.Messaging
{
  public class TarefaCreatedEventConsumer : IConsumer<TarefaCreatedEvent>
  {

    private readonly IHubContext<TarefasHub> _hubContext;
    public TarefaCreatedEventConsumer(IHubContext<TarefasHub> hubContext)
    {
      _hubContext = hubContext;
    }
    public async Task Consume(ConsumeContext<TarefaCreatedEvent> context)
    {
      var message = context.Message;
      Console.WriteLine($"Tarefa criada: {message.TarefaId}");
      await _hubContext.Clients.All.SendAsync("TarefaCriada", message);
    }
  }
}