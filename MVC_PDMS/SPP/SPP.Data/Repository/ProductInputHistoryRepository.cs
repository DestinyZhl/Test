using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SPP.Data.Repository
{
    public interface IProductInputHistoryRepository : IRepository<Product_Input_History>
    {

    }
    public class ProductInputHistoryRepository : RepositoryBase<Product_Input_History>, IProductInputHistoryRepository
    {

        public ProductInputHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}

