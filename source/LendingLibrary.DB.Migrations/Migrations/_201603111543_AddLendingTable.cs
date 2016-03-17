using FluentMigrator;
using Table =LendingLibrary.DB.DbConstants.Tables.LendingTable;
namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201603111543)]
    public class _201603111543_AddLendingTable : Migration
    {
        public override void Up()
        {
            Create.Table(Table.TableName)
             .WithColumn(Table.Columns.LedingId).AsGuid().NotNullable().PrimaryKey()
             .WithColumn(Table.Columns.PersonId).AsGuid().NotNullable().ForeignKey()
             .WithColumn(Table.Columns.ItemId).AsGuid().NotNullable().ForeignKey()
             .WithColumn(Table.Columns.DateBorrowed).AsDateTime().Nullable()
             .WithColumn(Table.Columns.DateReturned).AsDateTime().Nullable()
             .WithColumn(Table.Columns.LendingStatus).AsInt32();
        }

        public override void Down()
        {
        }
    }
}