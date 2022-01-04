using BusinessLayer.Interfaces;

using DataAccessLayer.Interfaces;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusinessLayer.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        // FIELDS
        IUnitOfWork unitOfWork;
        IDictionary<Type, object> handlersFactory;

        // CONSTRUCTORS
        public CommandProcessor(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.handlersFactory = new Dictionary<Type, object>();
        }

        // METHODS
        // create handler and call its method
        public Task<TResponse> ProcessAsync<THandler, TCommand, TResponse>(TCommand command)
            where THandler : ICommandHandler<TCommand, TResponse>, IUnitOfWorkSettable, new()
            where TCommand : ICommand<TResponse>
        {
            Type key = typeof(TCommand);

            // create lazy loaded handlers
            if (!handlersFactory.ContainsKey(key))
            {
                THandler handler = new THandler();
                handler.SetUnitOfWork(unitOfWork);
                handlersFactory.Add(key, handler);
            }

            return ((THandler)handlersFactory[key]).ExecuteAsync(command);
        }
    }
}
