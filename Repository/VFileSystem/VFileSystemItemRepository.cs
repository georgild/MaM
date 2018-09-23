using Models.VFileSystem;
using Repository.Base;
using Repository.Contexts;
using RepositoryContracts.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.VFileSystem {
    public class VFileSystemItemRepository : GenericRepository<VFileSystemItem>, IVFileSystemItemRepository {

        public VFileSystemItemRepository(Context context)
             : base(context) { }
    }
}
