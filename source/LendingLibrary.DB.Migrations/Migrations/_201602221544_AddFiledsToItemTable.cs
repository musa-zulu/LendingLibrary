using FluentMigrator;
using _ItemsTable = LendingLibrary.DB.DbConstants.Tables.ItemTable;
using _Common = LendingLibrary.DB.DbConstants.Tables.Common;
namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201602221544)]
    public class _201602221544_AddFiledsToItemTable : Migration
    {
        public override void Up()
        {
            Alter.Table(_ItemsTable.TableName)
                .AddColumn(_Common.Columns.DateCreated).AsDate().Nullable()
                .AddColumn(_Common.Columns.CreatedUsername).AsString(300).Nullable()
                .AddColumn(_Common.Columns.DateLastModified).AsDate().Nullable()
                .AddColumn(_Common.Columns.LastModifiedUsername).AsString();
        }

        public override void Down()
        { }
    }
}