using System;
using System.Configuration;
using System.Web.Http;
using FibonacciSolver;
using log4net;
using MassTransit;
using MessageBus;
using Models;

namespace RestApiApp.Api.Controllers
{
    public class RootController : ApiController
    {
        private readonly IFibonacciSolver _solver;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RootController));
        private readonly IMessageBus _messageBus;

        public RootController()
        {
            _solver = FibonacciSolverFactory.GetSolver();

            _messageBus = MessageBusFactory.GetMassTransitMessageBus();
        }


        [HttpGet]
        public bool GetNextFibonacciNumber(ulong x)
        {
            _logger.DebugFormat("Получено сообщение: Value = {0}", x);

            var nextNumber = _solver.GetNext(x);

            PublishContext(new ExecutingContext() {Value = nextNumber});

            return true;
        }

        private void PublishContext(ExecutingContext context)
        {
            try
            {
                _messageBus.Start();
                _messageBus.Publish<ExecutingContext>(context);
                _logger.DebugFormat("Сообщение отправлено в шину (Value = {0})", context.Value);
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Ошибка при отправке сообщения: {0}", ex);
            }
            finally
            {
                _messageBus.Stop();
            }
        }
    }
}
