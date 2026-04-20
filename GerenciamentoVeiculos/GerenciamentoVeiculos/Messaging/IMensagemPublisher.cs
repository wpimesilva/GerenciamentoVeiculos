namespace GerenciamentoVeiculos.Messaging;

public interface IMensagemPublisher
{
    Task PublicarAsync<T>(T mensagem);
}
