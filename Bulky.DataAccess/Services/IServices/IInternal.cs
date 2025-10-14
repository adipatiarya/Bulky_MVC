using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Services.IServices
{
    public interface IInternal
    {
        CategoryRepository CategoryRepository { get; }

        void Save();
    }
}
