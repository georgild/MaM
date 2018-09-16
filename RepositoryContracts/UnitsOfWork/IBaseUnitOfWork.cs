using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts.UnitsOfWork {
    public interface IBaseUnitOfWork : IDisposable {
        Task<int> Save();
    }
}
