using GestaoTarefas.Application;
using GestaoTarefas.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoTarefas.Api.Controllers
{
  [ApiController]
  // [Authorize]
  [Route("api/[controller]")]
  public class TarefaController : ControllerBase
  {
    private readonly ITarefaService _tarefaService;

    public TarefaController(ITarefaService tarefaService)
    {
      _tarefaService = tarefaService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTarefa(int id)
    {
      var tarefa = await _tarefaService.GetTarefaByIdAsync(id);
      if (tarefa == null)
        return NotFound();

      return Ok(tarefa);
    }

    [HttpGet]
    public async Task<IActionResult> GetTarefas()
    {
      var tarefas = await _tarefaService.GetAllTarefasAsync();
      return Ok(tarefas);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTarefa([FromBody] CreateTarefaDto createTarefaDto)
    {
      var id = await _tarefaService.CreateTarefaAsync(createTarefaDto);
      return CreatedAtAction(nameof(GetTarefa), new { id }, createTarefaDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTarefa(int id, [FromBody] UpdateTarefaDto updateTarefaDto)
    {
      if (id != updateTarefaDto.Id)
        return BadRequest();

      await _tarefaService.UpdateTarefaAsync(updateTarefaDto);
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTarefa(int id)
    {
      await _tarefaService.DeleteTarefaAsync(id);
      return NoContent();
    }
  }
}
