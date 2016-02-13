using System.Data.Entity;
using LendingLibrary.Core.Domain;
using LendingLibrary.DB.Mapping;

namespace LendingLibrary.DB
{
    public interface ILendingLibraryDbContext
    {
        IDbSet<Item> Items { get; set; }
    }

    public class LendingLibraryDbContext : DbContext, ILendingLibraryDbContext
    {
        static LendingLibraryDbContext()
        {
            Database.SetInitializer<LendingLibraryDbContext>(null); 
        }

        public LendingLibraryDbContext(string nameOrConnectionString = null)
            :base(nameOrConnectionString ?? "Name=LendingLibraryWebContext")
        {
        }

        public IDbSet<Item> Items { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var config = modelBuilder.Configurations;
            config.Add(new ItemMap());
        }
    }
}