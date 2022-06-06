using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Interfaces
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly DataContext _context;

        public CoverTypeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CoverType obj)
        {
            _context.CoverTypes.Update(obj);
        }
    }
}
