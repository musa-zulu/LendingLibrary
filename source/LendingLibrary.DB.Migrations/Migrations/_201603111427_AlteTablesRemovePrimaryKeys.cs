using FluentMigrator;
using Table = LendingLibrary.DB.DbConstants.Tables;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201603111427)]
    public class _201603111427_AlteTablesRemovePrimaryKeys : Migration
    {
        public override void Up()
        {
           // Delete.Column(Table.PersonTable.Columns.PersonId).FromTable(Table.PersonTable.TableName);
           // Delete.Column(Table.ItemTable.Columns.ItemId).FromTable(Table.ItemTable.TableName);
        }

        public override void Down()
        {
        }
    }
}


