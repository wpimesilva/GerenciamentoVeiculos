using GerenciamentoVeiculos.DTOs;
using GerenciamentoVeiculos.Exceptions;
using GerenciamentoVeiculos.Messaging;
using GerenciamentoVeiculos.Models;
using GerenciamentoVeiculos.Repositories;
using GerenciamentoVeiculos.Repositories.Interfaces;
using GerenciamentoVeiculos.Services.Interfaces;

namespace GerenciamentoVeiculos.Services;

public class VeiculoService : IVeiculoService
{
    private readonly IVeiculosRepository _veiculosRepository;
    private readonly IProprietarioRepository _proprietarioRepository;
    private readonly IMensagemPublisher _mensagemPublisher;

    public VeiculoService(IVeiculosRepository veiculosRepository,
        IProprietarioRepository proprietarioRepository,
        IMensagemPublisher mensagemPublisher)
    {
        _veiculosRepository = veiculosRepository;
        _proprietarioRepository = proprietarioRepository;
        _mensagemPublisher = mensagemPublisher;
    }
    public async Task<List<VeiculoRespostaDto>> ObterTodosAsync()
    {
        var veiculos = await _veiculosRepository.ObterTodosAsync();
        return veiculos.Select(MapearParaDTO).ToList();

    }




    public async Task<VeiculoRespostaDto?> ObterPorIdAsync(Guid id)
    {
        var veiculos = await _veiculosRepository.ObterPorIdAsync(id)
            ?? throw new NaoEncontradoException("Proprietario não encontrado");

        return MapearParaDTO(veiculos);

    }

    public async Task<VeiculoRespostaDto> AdicionarAsync(VeiculoRespostaDto dto)
    {
        var proprietario = await _proprietarioRepository.ObterPorIdAsync(dto.ProprietarioId)
            ?? throw new NaoEncontradoException("Proprietario não encontrado");

        var placaExistente = await _veiculosRepository.ObterPorPlacaAsync(dto.Placa);

        if (placaExistente is not null)
            throw new RegraNegocioException("Já existe veiculo ativo com essa placa");


        var veiculo = new Veiculo
        {
            Ano = dto.Ano,
            Ativo = true,
            Modelo = dto.Modelo.Trim(),
            Placa = dto.Placa.Trim().ToUpper(),
            ProprietarioId = dto.ProprietarioId
        };

        await _veiculosRepository.AdicionarAsync(veiculo);

        await _mensagemPublisher.PublicarAsync(new VeiculoCriadoMensagem
        {
            VeiculoId = veiculo.Id,
            Placa = veiculo.Placa,
            Modelo = veiculo.Modelo,
            ProprietarioId = veiculo.ProprietarioId,
            CriadoEm = DateTime.UtcNow
        }
            );

        return MapearParaDTO(veiculo);
    }
    public async Task<VeiculoRespostaDto> CriarAsync(VeiculoCriacaoDto dto)
    {
        var proprietario = await _proprietarioRepository.ObterPorIdAsync(dto.ProprietarioId)
            ?? throw new RegraNegocioException("Proprietário informado não existe.");

        var placaExistente = await _veiculosRepository.ObterPorPlacaAsync(dto.Placa.Trim().ToUpper());
        if (placaExistente is not null)
            throw new RegraNegocioException("Já existe veículo ativo com esta placa.");

        var veiculo = new Veiculo
        {
            Placa = dto.Placa.Trim().ToUpper(),
            Modelo = dto.Modelo.Trim(),
            Ano = dto.Ano,
            ProprietarioId = dto.ProprietarioId,
            Ativo = true
        };

        await _veiculosRepository.AdicionarAsync(veiculo);

        /*Comentado para poder funcionar. Não tem servidor configurado de Azure Service Bus
         * await _mensagemPublisher.PublicarAsync(new VeiculoCriadoMensagem
        {
            VeiculoId = veiculo.Id,
            Placa = veiculo.Placa,
            Modelo = veiculo.Modelo,
            ProprietarioId = veiculo.ProprietarioId,
            CriadoEm = DateTime.UtcNow
        });*/

        veiculo.Proprietario = proprietario;

        return MapearParaDTO(veiculo);
    }

    public async Task<VeiculoRespostaDto> AtualizarAsync(Guid id, VeiculoCriacaoDto dto)
    {
            
        var veiculo = await _veiculosRepository.ObterPorIdAsync(id)
            ?? throw new NaoEncontradoException("Veículo não encontrado.");

        var proprietario = await _proprietarioRepository.ObterPorIdAsync(dto.ProprietarioId)
            ?? throw new RegraNegocioException("Proprietário informado não existe.");

        veiculo.Placa = dto.Placa.Trim().ToUpper();
        veiculo.Modelo = dto.Modelo.Trim();
        veiculo.Ano = dto.Ano;
        veiculo.ProprietarioId = dto.ProprietarioId;

        await _veiculosRepository.AtualizaAsync(veiculo);

        veiculo.Proprietario = proprietario;

        return MapearParaDTO(veiculo);
    }

    public async Task ExcluirAsync(Guid id)
    {
        var veiculo = await _veiculosRepository.ObterPorIdAsync(id)
            ?? throw new NaoEncontradoException("Veículo não encontrado.");

        veiculo.Ativo = false;
        await _veiculosRepository.AtualizaAsync(veiculo);
    }

    public async Task<List<VeiculoRespostaDto>> BuscarAsync(string? placa, string? modelo, string? nomeProprietario)
    {
        var veiculos = await _veiculosRepository.BuscarAsync(placa, modelo, nomeProprietario);
        return veiculos.Select(MapearParaDTO).ToList();
    }

    

    


    public static VeiculoRespostaDto MapearParaDTO(Veiculo veiculo)
    {
        return new VeiculoRespostaDto
        {
            Ano = veiculo.Ano,
            Modelo = veiculo.Modelo,
            NomeProprietario = veiculo.Proprietario.Nome,
            Placa = veiculo.Placa,
            ProprietarioId = veiculo.Proprietario.Id
        };
    }

}
