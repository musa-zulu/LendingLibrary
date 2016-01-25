using System.Collections.Generic;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.DB
{
    interface ITempDbContex
    {
        List<Person> People { get; set; }
    }
    public class TempDbContext : ITempDbContex
    {
        public TempDbContext()
        {
            People = new List<Person>();
        }
        public List<Person> People { get; set; }
    }
}