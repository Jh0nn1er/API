using Microsoft.EntityFrameworkCore;
using WebAPIProduco.Model;


namespace WebAPIProduco.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options)
        :base(options)
        {

        }
        public DbSet<Form> Forms{get; set; }
        
    }
}