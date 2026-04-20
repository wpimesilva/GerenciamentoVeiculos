using Microsoft.EntityFrameworkCore.InMemory.Design.Internal;

namespace GerenciamentoVeiculos.Exceptions
{
    public class NaoEncontradoException : Exception

    {
        public NaoEncontradoException(string message) : base(message) { }
    }
}
