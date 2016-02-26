using FluentMigrator;
using PersonTable = LendingLibrary.DB.DbConstants.Tables.PersonTable;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201602261517)]
    public class _201602261517_AddFielsToPersonTable : Migration
    {
        public override void Up()
        {
            Alter.Table(PersonTable.TableName)
                .AddColumn(PersonTable.Columns.PhoneNumber).AsInt64().Nullable();
        }

        public override void Down()
        {
        }
    }
}
