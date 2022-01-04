using BusinessLayer.Interfaces;

using DataAccessLayer.Interfaces;

using System;
using System.Collections.Generic;

namespace BusinessLayer.Queries
{
    public class QueryProcessor : IQueryProcessor
    {
        // FIELDS
        IUnitOfWork unitOfWork;
        IDictionary<Type, object> handlersFactory;

        // CONSTRUCTORS
        public QueryProcessor(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.handlersFactory = new Dictionary<Type, object>();
        }

        // METHODS
        // create handler and call its method
        public TResponse Process<THandler ,TQuery, TResponse>(TQuery query) 
            where THandler: IQueryHandler<TQuery, TResponse>, IUnitOfWorkSettable, new() 
            where TQuery : IQuery<TResponse>
        {
            Type key = typeof(TQuery);

            // create laly loaded handlers
            if (!handlersFactory.ContainsKey(key))
            {
                THandler handler = new THandler();
                handler.SetUnitOfWork(unitOfWork);
                handlersFactory.Add(key, handler);
            }

            return ((THandler)handlersFactory[key]).Handle(query);
        }
    }
}
