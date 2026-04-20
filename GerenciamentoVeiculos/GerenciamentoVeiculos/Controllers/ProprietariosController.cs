using GerenciamentoVeiculos.DTOs;
using GerenciamentoVeiculos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace VehicleOwner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProprietariosController : ControllerBase
{
    private readonly IProprietarioService _service;

    public ProprietariosController(IProprietarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
        => Ok(await _service.ObterTodosAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
        => Ok(await _service.ObterPorIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ProprietarioCriacaoDto dto)
    {
        var criado = await _service.AdicionarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ProprietarioCriacaoDto dto)
        => Ok(await _service.AtualizaAsync(id, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}
