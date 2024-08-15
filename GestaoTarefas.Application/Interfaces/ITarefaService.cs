using GestaoTarefas.Application;
using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Domain.Interfaces
{
    public interface ITarefaService
    {
        Task<Tarefa?> GetTarefaByIdAsync(int id);
        Task<IEnumerable<Tarefa>> GetAllTarefasAsync();
        Task<int> CreateTarefaAsync(CreateTarefaDto createTarefaDto);
        Task UpdateTarefaAsync(UpdateTarefaDto updateTarefaDto);
        Task DeleteTarefaAsync(int id);
    }
}
