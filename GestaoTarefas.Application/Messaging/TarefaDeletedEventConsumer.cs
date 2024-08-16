using GestaoTarefas.Application.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace GestaoTarefas.Application.Messaging
{
  public class TarefaDeletedEventConsumer : IConsumer<TarefaDeletedEvent>
  {
    private readonly IHubContext<TarefasHub> _hubContext;

    public TarefaDeletedEventConsumer(IHubContext<TarefasHub> hubContext)
    {
      _hubContext = hubContext;
    }
    public async Task Consume(ConsumeContext<TarefaDeletedEvent> context)
    {
      var message = context.Message;
      Console.WriteLine($"Tarefa excluída: {message.TarefaId}, Data de exclusão: {message.DataExclusao}");
      await _hubContext.Clients.All.SendAsync("TarefaExcluida", message.TarefaId, message.DataExclusao);
    }
  }
}
