using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        SPPContext Get();
    }
}
