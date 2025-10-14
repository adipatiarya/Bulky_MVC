using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Services.IServices;
using Bulky.Models;

namespace Bulky.DataAccess.Services
{
    public class InternalUnit:IInternal
    {
        private ApplicationDbContext _context;
        public CategoryRepository CategoryRepository  { get; private set; }


        public InternalUnit(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
        }
        public void Save()
        {
           _context.SaveChanges();
        }
    }
}
