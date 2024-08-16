using Microsoft.AspNetCore.SignalR;

namespace GestaoTarefas.Application.Messaging
{
  public class TarefasHub : Hub
  {
    public async Task NotifyTaskCreation(int tarefaId, string tarefaName)
    {
      await Clients.All.SendAsync("TarefaCriada", tarefaId, tarefaName);
    }

    public async Task NotifyTaskDeletion(int tarefaId, DateTime dataExclusao)
    {
      await Clients.All.SendAsync("TarefaExcluida", tarefaId, dataExclusao);
    }
    public async Task NotifyTaskUpdate(int tarefaId, object tarefa)
    {
      await Clients.All.SendAsync("TarefaAtualizada", tarefaId, tarefa);
    }
  }
}
