using GestaoTarefas.Application;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Interfaces;

public class TarefaService
{
  private readonly ITarefaRepository _tarefaRepository;

  public TarefaService(ITarefaRepository tarefaRepository)
  {
    _tarefaRepository = tarefaRepository;
  }

  public async Task<Tarefa?> GetTarefaByIdAsync(int id)
  {
    return null;
  }

  public async Task<IEnumerable<Tarefa>> GetAllTarefasAsync()
  {
    return null;
  }

  public async Task<int> CreateTarefaAsync(CreateTarefaDto createTarefaDto)
  {
    var tarefa = new Tarefa
    {
      Titulo = createTarefaDto.Titulo,
      Descricao = createTarefaDto.Descricao,
      DataVencimento = createTarefaDto.DataVencimento,
      Status = createTarefaDto.Status,
      DataCriacao = DateTime.UtcNow,
      DataAlteracao = DateTime.UtcNow
    };

    return default;
  }

  public async Task UpdateTarefaAsync(UpdateTarefaDto updateTarefaDto)
  {
    // var tarefa = new Tarefa
    // {
    //   Id = updateTarefaDto.Id,
    //   Titulo = updateTarefaDto.Titulo,
    //   Descricao = updateTarefaDto.Descricao,
    //   DataVencimento = updateTarefaDto.DataVencimento,
    //   Status = updateTarefaDto.Status,
    //   DataAlteracao = DateTime.UtcNow
    // };

    // await _tarefaRepository.UpdateAsync(tarefa);
  }

  public async Task DeleteTarefaAsync(int id)
  {
    // await _tarefaRepository.DeleteAsync(id);
  }
}
