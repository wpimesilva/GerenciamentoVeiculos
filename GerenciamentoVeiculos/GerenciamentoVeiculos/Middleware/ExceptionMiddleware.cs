using System.Net;
using System.Text.Json;
using GerenciamentoVeiculos.Exceptions;

namespace GerenciamentoVeiculos.Middleware
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
        
        public ExceptionMiddleware (RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    NaoEncontradoException => (int)HttpStatusCode.NotFound,
                    ValidacaoException => (int)HttpStatusCode.BadRequest,
                    RegraNegocioException => (int)HttpStatusCode.InternalServerError

                };

                context.Response.StatusCode = statusCode;

                var response = new
                {
                    erro = ex.Message,
                    statusCode
                };

                await context.Response.WriteAsync (JsonSerializer.Serialize (response));
            }
        }

    }
}
