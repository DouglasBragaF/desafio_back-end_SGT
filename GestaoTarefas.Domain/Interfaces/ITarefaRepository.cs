using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Domain.Interfaces
{
  public interface ITarefaRepository
  {
    Task<Tarefa?> GetByIdAsync(int id);
    Task<IEnumerable<Tarefa>> GetAllAsync();
    Task<int> AddAsync(Tarefa tarefa);
    Task UpdateAsync(Tarefa tarefa);
    Task DeleteAsync(int id);
  }
}