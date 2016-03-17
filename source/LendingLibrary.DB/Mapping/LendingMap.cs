using System.Data.Entity.ModelConfiguration;
using LendingLibrary.Core.Domain;
using LendingTable = LendingLibrary.DB.DbConstants.Tables.LendingTable;

namespace LendingLibrary.DB.Mapping
{
   public class LendingMap : EntityTypeConfiguration<Lending>
    {
        public LendingMap()
        {
            HasKey(it => it.LedingId);

            ToTable(LendingTable.TableName);
            Property(p => p.ItemId).HasColumnName(LendingTable.Columns.ItemId);
            Property(p => p.PersonId).HasColumnName(LendingTable.Columns.PersonId);
            Property(p => p.DateReturned).HasColumnName(LendingTable.Columns.DateReturned);
            Property(p => p.DateBorrowed).HasColumnName(LendingTable.Columns.DateBorrowed);
            Property(p => p.LendingStatus).HasColumnName(LendingTable.Columns.LendingStatus);
        }
    }
}
