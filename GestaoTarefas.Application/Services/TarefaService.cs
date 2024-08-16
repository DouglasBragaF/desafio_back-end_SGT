using MassTransit;
using GestaoTarefas.Application;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Interfaces;
using GestaoTarefas.Application.Events;

public class TarefaService : ITarefaService
{
  private readonly ITarefaRepository _tarefaRepository;

  // private readonly IBus _bus;

  public TarefaService(ITarefaRepository tarefaRepository)
  {
    // _bus = bus;
    _tarefaRepository = tarefaRepository;
  }

  public async Task<Tarefa?> GetTarefaByIdAsync(int id)
  {
    return await _tarefaRepository.GetByIdAsync(id);
  }

  public async Task<IEnumerable<Tarefa>> GetAllTarefasAsync()
  {
    return await _tarefaRepository.GetAllAsync();
  }

  public async Task<int> CreateTarefaAsync(CreateTarefaDto createTarefaDto)
  {
    // Converte o DTO para uma entidade Tarefa
    var tarefa = ConvertToEntity(createTarefaDto);
    var result = await _tarefaRepository.AddAsync(tarefa);

    // await PublishEventAsync(new TarefaCreatedEvent
    // {
    //   TarefaId = result,
    //   Tarefa = createTarefaDto

    // });

    // Persiste a entidade no repositório
    return result;
  }

  public async Task UpdateTarefaAsync(UpdateTarefaDto updateTarefaDto)
  {
    // Recupera a tarefa existente
    var tarefaExistente = await _tarefaRepository.GetByIdAsync(updateTarefaDto.Id);
    if (tarefaExistente == null)
    {
      throw new Exception("Tarefa não encontrada.");
    }

    // Atualiza a entidade Tarefa com os dados do DTO
    ConvertToEntity(updateTarefaDto, tarefaExistente);

    // Atualiza a entidade no repositório
    await _tarefaRepository.UpdateAsync(tarefaExistente);
  }

  public async Task DeleteTarefaAsync(int id)
  {
    var result = await _tarefaRepository.GetByIdAsync(id);
    if (result == null)
    {
      throw new Exception("Tarefa não encontrada.");
    }

    // await PublishEventAsync(new TarefaDeletedEvent
    // {
    //   TarefaId = id,
    //   DataExclusao = DateTime.UtcNow
    // });
    // Remove a entidade do repositório
    await _tarefaRepository.DeleteAsync(id);
  }

  private Tarefa ConvertToEntity(CreateTarefaDto dto)
  {
    return new Tarefa
    {
      Titulo = dto.Titulo,
      Descricao = dto.Descricao,
      DataVencimento = dto.DataVencimento,
      Status = dto.Status,
      DataCriacao = DateTime.UtcNow,
      DataAlteracao = DateTime.UtcNow
    };
  }

  private void ConvertToEntity(UpdateTarefaDto dto, Tarefa tarefaExistente)
  {
    tarefaExistente.Titulo = dto.Titulo;
    tarefaExistente.Descricao = dto.Descricao;
    tarefaExistente.DataVencimento = dto.DataVencimento;
    tarefaExistente.Status = dto.Status;
    tarefaExistente.DataAlteracao = DateTime.UtcNow;
  }

  // private async Task PublishEventAsync<T>(T evento) where T : class
  // {
  //   try
  //   {
  //     await _bus.Publish(evento);
  //   }
  //   catch (Exception ex)
  //   {
  //     throw new Exception("Erro ao publicar o evento.", ex);
  //   }
  // }
}
