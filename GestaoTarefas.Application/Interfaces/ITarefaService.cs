using GestaoTarefas.Application;
using GestaoTarefas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

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
