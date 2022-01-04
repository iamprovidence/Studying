using BusinessLayer.Interfaces;

using DataAccessLayer.Interfaces;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

namespace BusinessLayer.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        // FIELDS
        IMapper mapper;
        IUnitOfWork unitOfWork;
        IDictionary<Type, object> handlersFactory;

        // CONSTRUCTORS
        public CommandProcessor(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.handlersFactory = new Dictionary<Type, object>();
        }

        // METHODS
        // create handler and call its method
        public Task<TResponse> ProcessAsync<THandler, TCommand, TResponse>(TCommand command)
            where THandler : ICommandHandler<TCommand, TResponse>, IUnitOfWorkSettable, IMapperSettable, new()
            where TCommand : ICommand<TResponse>
        {
            Type key = typeof(TCommand);

            // create lazy loaded handlers
            if (!handlersFactory.ContainsKey(key))
            {
                THandler handler = new THandler();
                handler.SetUnitOfWork(unitOfWork);
                handler.SetMapper(mapper);
                handlersFactory.Add(key, handler);
            }

            return ((THandler)handlersFactory[key]).ExecuteAsync(command);
        }
    }
}
