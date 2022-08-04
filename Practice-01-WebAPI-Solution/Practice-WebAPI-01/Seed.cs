using Practice_WebAPI_01.Data;
using Practice_WebAPI_01.Models;

namespace Practice_WebAPI_01
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;
        }
        public void SeedDataContext()
        {
            // Only add the specified content if it doesn't currently exist in db
            if (_context.Heroes.Where(x => x.UserName == "Leif").FirstOrDefault() 
                == null)
            {
                var heroes = new List<Hero>()
                {
                    new Hero()
                    {
                        Id = 0,
                        UserName = "Leif",
                        Credit = 1000
                    }
                };

                _context.Heroes.AddRange(heroes);
                _context.SaveChanges();
            }
        }
    }

}

