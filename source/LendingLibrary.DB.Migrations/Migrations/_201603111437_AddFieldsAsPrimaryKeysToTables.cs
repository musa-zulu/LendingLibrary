using FluentMigrator;
using Table = LendingLibrary.DB.DbConstants.Tables;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201603111437)]
    public class _201603111437_AddFieldsAsPrimaryKeysToTables : Migration
    {
        public override void Up()
        {
            Alter.Table(Table.PersonTable.TableName)
                .AddColumn(Table.PersonTable.Columns.PersonId)
                .AsGuid()
                .NotNullable()
                .PrimaryKey();

            Alter.Table(Table.ItemTable.TableName)
              .AddColumn(Table.ItemTable.Columns.ItemId)
              .AsGuid()
              .NotNullable()
              .PrimaryKey();

        }

        public override void Down()
        {
        }
    }
}
