using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace GerenciamentoVeiculos.Messaging;

public class AzureServiceBusPublisher : IMensagemPublisher, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;

    public AzureServiceBusPublisher(IConfiguration configuration)
    {
        var connectionString = configuration["AzureServiceBus:ConnectionString"] ?? throw new InvalidOperationException("AzureServiceBus:ConnectionString não configurada");

        _client = new ServiceBusClient(connectionString);
        _sender = _client.CreateSender(NomesFilas.VeiculoCriado);
    }

    public async Task PublicarAsync<T>(T mensagem)
    {
        var payload = JsonSerializer.Serialize(mensagem);
        var serviceBusMessage = new ServiceBusMessage(payload)
        {
            MessageId = Guid.NewGuid().ToString(),
            Subject = typeof(T).Name
        };
        await _sender.SendMessageAsync(serviceBusMessage);
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
        await _client.DisposeAsync();
    }
}
