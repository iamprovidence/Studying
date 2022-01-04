using AutoMapper;

using BusinessLayer.Interfaces;

using DataAccessLayer.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Queries
{
    public class QueryProcessor : IQueryProcessor
    {
        // FIELDS
        IMapper mapper;
        IUnitOfWork unitOfWork;
        IDictionary<Type, object> handlersFactory;

        // CONSTRUCTORS
        public QueryProcessor(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.handlersFactory = new Dictionary<Type, object>();
        }

        // METHODS
        // create handler and call its method
        public Task<TResponse> ProcessAsync<THandler ,TQuery, TResponse>(TQuery query) 
            where THandler: IQueryHandler<TQuery, TResponse>, IUnitOfWorkSettable, IMapperSettable, new() 
            where TQuery : IQuery<TResponse>
        {
            Type key = typeof(TQuery);

            // create lazy loaded handlers
            if (!handlersFactory.ContainsKey(key))
            {
                THandler handler = new THandler();
                handler.SetUnitOfWork(unitOfWork);
                handler.SetMapper(mapper);
                handlersFactory.Add(key, handler);
            }

            return ((THandler)handlersFactory[key]).HandleAsync(query);
        }
    }
}
