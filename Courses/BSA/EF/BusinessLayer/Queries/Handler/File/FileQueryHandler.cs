using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.File;

using Core.DataTransferObjects.RabbitMQ;

using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using DataAccessLayer.Interfaces;

namespace BusinessLayer.Queries.Handler.File
{
    public class FileQueryHandler : IQueryHandler<FileContentQuery, IEnumerable<WorkerData>>, IUnitOfWorkSettable
    {
        public IEnumerable<WorkerData> Handle(FileContentQuery query)
        {
            // checking
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (!System.IO.File.Exists(query.FilePath)) return null;

            // read file
            List<WorkerData> fileContent = new List<WorkerData>();
            using (StreamReader sr = new StreamReader(query.FilePath))
            {
                string objectJson = String.Empty;
                while ((objectJson = sr.ReadLine()) != null)
                {
                    WorkerData result = JsonConvert.DeserializeObject<WorkerData>(objectJson);
                    fileContent.Add(result);
                }
            }

            // sorting
            fileContent.Sort((a, b) =>
            {
                int v = a.Date.CompareTo(b.Date);
                return query.SortingOrder == Enums.SortingOrder.Asc ? v : -v;
            });
            
            return fileContent;
        }
        
        void IUnitOfWorkSettable.SetUnitOfWork(IUnitOfWork unitOfWork)
        {
        }
    }
}
