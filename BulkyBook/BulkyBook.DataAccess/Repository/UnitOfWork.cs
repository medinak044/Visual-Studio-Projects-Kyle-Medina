using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.Interfaces;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public ICategoryRepository Category { get ; private set; }

        public ICoverTypeRepository CoverType { get; private set; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            CoverType = new CoverTypeRepository(_context);
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
