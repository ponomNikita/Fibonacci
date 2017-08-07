using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FibonacciSolver;
using log4net;
using MassTransit;
using MessageBus;
using Models;

namespace MessageBusApp
{
    public class FibonacciService : IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FibonacciService));
        private static readonly ManualResetEvent SignalEvent = new ManualResetEvent(false);
        private static int _maxIterationCount;
        private static int _iteration;
        private static readonly RequestSender RequestSender;
        private static readonly IFibonacciSolver Solver;
        private readonly IMessageBus _messageBus;
        private static ulong _result;

        static FibonacciService()
        {
            Solver = FibonacciSolverFactory.GetSolver();

            var rootApiUrl = ConfigurationManager.AppSettings["rootApiAppUrl"];

            rootApiUrl = !string.IsNullOrEmpty(rootApiUrl) ? rootApiUrl : "http://localhost:8888";

            RequestSender = new RequestSender(rootApiUrl);
        }


        public FibonacciService()
        {
            _messageBus = MessageBusFactory.GetMassTransitMessageBus(HandleResponse);
            _messageBus.Start();
        }

        public ulong GetNumber(int n)
        {
            SignalEvent.Reset();
            var newThread = new Thread(() => StartCalculations(n));
            newThread.Start();

            SignalEvent.WaitOne();
            SignalEvent.Reset();

            return _result;
        }

        private void StartCalculations(int n)
        {
            _maxIterationCount = n;
            _iteration = 1;
            _result = 0;

            Logger.DebugFormat("Итерация {0}", _iteration);

            RequestSender.SentGetRequest<bool>("api/Root?x=" + 0);
        }

        private static Task HandleResponse(ConsumeContext<ExecutingContext> context)
        {
            Logger.DebugFormat("Получено сообщение от RestApiApp: Value = {0}",
                        context.Message.Value);

            if (_iteration >= _maxIterationCount)
            {
                _result = context.Message.Value;
                SignalEvent.Set();

                return Task.FromResult(0);
            }
            else
            {
                Logger.DebugFormat("Итерация {0}", ++_iteration);

                ulong next = Solver.GetNext(context.Message.Value);

                if (_iteration >= _maxIterationCount)
                {
                    _result = next;
                    SignalEvent.Set();

                    return Task.FromResult(0);
                }
                else
                {
                    Logger.DebugFormat("Итерация {0}", ++_iteration);
                    RequestSender.SentGetRequest<bool>("api/Root?x=" + next);
                    return Task.FromResult(0);
                }
            }
        }

        public void Dispose()
        {
            _messageBus.Stop();
            SignalEvent.Dispose();
        }
    }
}
