using GerenciamentoVeiculos.DTOs;
using GerenciamentoVeiculos.Exceptions;
using GerenciamentoVeiculos.Models;
using GerenciamentoVeiculos.Repositories.Interfaces;
using GerenciamentoVeiculos.Services.Interfaces;
using System.Net.NetworkInformation;

namespace GerenciamentoVeiculos.Services;


public class ProprietarioService : IProprietarioService
{
    private readonly IProprietarioRepository _proprietarioRepository;

    public ProprietarioService(IProprietarioRepository proprietarioRepository)
    {
        _proprietarioRepository = proprietarioRepository;
    }
    public async Task<List<ProprietarioRespostaDto>> ObterTodosAsync()
    {
        var proprietarios = await _proprietarioRepository.ObterTodosAsync();
        return proprietarios.Select(MapearParaDTO).ToList();

    }

    public async Task<ProprietarioRespostaDto?> ObterPorIdAsync(Guid id)
    {
        var proprietarios = await _proprietarioRepository.ObterPorIdAsync(id)
            ?? throw new NaoEncontradoException("Proprietario não encontrado");

        return MapearParaDTO(proprietarios);

    }

    public async Task<ProprietarioRespostaDto> AdicionarAsync(ProprietarioCriacaoDto dto)
    {
        
        var proprietario = new Proprietario
        {
            Nome = dto.Nome.Trim(),
            Cpf = dto.Cpf.Trim(),
            DataNascimento = dto.DataNascimento,
            Ativo = true
        };

        await _proprietarioRepository.AdicionarAsync(proprietario);

        return MapearParaDTO(proprietario);

    }

    public async Task<ProprietarioRespostaDto> AtualizaAsync(Guid id, ProprietarioCriacaoDto dto)
    {
       
        var proprietario = await _proprietarioRepository.ObterPorIdAsync(id)
            ?? throw new NaoEncontradoException("Proprietario não encontrado");

        proprietario.Nome = dto.Nome.Trim();
        proprietario.Cpf = dto.Cpf.Trim();
        proprietario.DataNascimento = dto.DataNascimento;


        await _proprietarioRepository.AtualizaAsync(proprietario);

        return MapearParaDTO(proprietario);

    }

    public async Task<ProprietarioRespostaDto> ExcluirAsync(Guid id)
    {
      
        var possuiVeiculos = await _proprietarioRepository.PossuiVeiculosAtivoAsync(id);

        if (possuiVeiculos)
            throw new RegraNegocioException("Não é permitido excluir um proprietário com veiculos ativos cadastrados.");

                

        var proprietario = await _proprietarioRepository.ObterPorIdAsync(id)
            ?? throw new NaoEncontradoException("Proprietario não encontrado");

        proprietario.Ativo = false;
        

        await _proprietarioRepository.AtualizaAsync(proprietario);

        return MapearParaDTO(proprietario);

    }




    public static ProprietarioRespostaDto MapearParaDTO(Proprietario proprietario)
    {
        return new ProprietarioRespostaDto
        {
            Cpf = proprietario.Cpf,
            DataNascimento = proprietario.DataNascimento,
            Id = proprietario.Id,
            Nome = proprietario.Nome
        };
    }
}
