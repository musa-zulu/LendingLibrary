using FluentMigrator;
using PersonTable = LendingLibrary.DB.DbConstants.Tables.PersonTable;
using ItemTable = LendingLibrary.DB.DbConstants.Tables.ItemTable;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201603071720)]
    public class _201603071720_AlterTablePersonAddTileField : Migration
    {
        public override void Up()
        {
            Alter.Table(PersonTable.TableName)
                .AddColumn(ItemTable.Columns.ItemId).AsInt32().NotNullable()
                    .ForeignKey(ItemTable.TableName, ItemTable.Columns.ItemId)
            .AddColumn(PersonTable.Columns.Title).AsInt32().Nullable();
        }

        public override void Down()
        {
        }
    }
}