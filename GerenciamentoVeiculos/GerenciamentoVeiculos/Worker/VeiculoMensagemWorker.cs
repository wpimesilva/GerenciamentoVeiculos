using Azure.Messaging.ServiceBus;
using GerenciamentoVeiculos.Messaging;
using System.Text.Json;


namespace VehicleOwner.Api.Workers;

public class VeiculoMensagemWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<VeiculoMensagemWorker> _logger;

    private ServiceBusProcessor? _processor;
    private ServiceBusClient? _client;

    public VeiculoMensagemWorker(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<VeiculoMensagemWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = _configuration["AzureServiceBus:ConnectionString"]
            ?? throw new InvalidOperationException("AzureServiceBus:ConnectionString não configurada.");

        _client = new ServiceBusClient(connectionString);

        _processor = _client.CreateProcessor(
            NomesFilas.VeiculoCriado,
            new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1
            });

        _processor.ProcessMessageAsync += ProcessarMensagemAsync;
        _processor.ProcessErrorAsync += ProcessarErroAsync;

        await _processor.StartProcessingAsync(cancellationToken);
        await base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    private async Task ProcessarMensagemAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var body = args.Message.Body.ToString();
            var mensagem = JsonSerializer.Deserialize<VeiculoCriadoMensagem>(body);

            _logger.LogInformation("Mensagem recebida. VeiculoId: {VeiculoId}, Placa: {Placa}",
                mensagem?.VeiculoId, mensagem?.Placa);

            using var scope = _scopeFactory.CreateScope();

            await Task.Delay(300);

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar mensagem {MessageId}", args.Message.MessageId);

            if (args.Message.DeliveryCount >= 5)
            {
                await args.DeadLetterMessageAsync(
                    args.Message,
                    "FalhaProcessamento",
                    "Falha após múltiplas tentativas.");
            }
            else
            {
                await args.AbandonMessageAsync(args.Message);
            }
        }
    }

    private Task ProcessarErroAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception,
            "Erro no Service Bus. Entity: {EntityPath}. Source: {ErrorSource}",
            args.EntityPath, args.ErrorSource);

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processor is not null)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
        }

        if (_client is not null)
        {
            await _client.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}
