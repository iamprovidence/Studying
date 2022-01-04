using BusinessLayer.Interfaces;

using DataAccessLayer.Interfaces;

using System;
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
        public TResponse Process<THandler, TCommand, TResponse>(TCommand command)
            where THandler : ICommandHandler<TCommand, TResponse>, IUnitOfWorkSettable, new()
            where TCommand : ICommand<TResponse>
        {
            Type key = typeof(TCommand);

            // create laly loaded handlers
            if (!handlersFactory.ContainsKey(key))
            {
                THandler handler = new THandler();
                handler.SetUnitOfWork(unitOfWork);
                handlersFactory.Add(key, handler);
            }

            return ((THandler)handlersFactory[key]).Execute(command);
        }
    }
}
