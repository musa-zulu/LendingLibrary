using FluentMigrator;
using _Common = LendingLibrary.DB.DbConstants.Tables.Common;
namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201602221423)]
    public class _201602221423_AddFieldsToPersonTable : Migration
    {
        public override void Up()
        {
            Alter.Table(DbConstants.Tables.PersonTable.TableName)
                .AddColumn(_Common.Columns.DateCreated).AsDate().Nullable()
                .AddColumn(_Common.Columns.CreatedUsername).AsString(300).Nullable()
                .AddColumn(_Common.Columns.DateLastModified).AsDate().Nullable()
                .AddColumn(_Common.Columns.LastModifiedUsername).AsString().Nullable();
        }

        public override void Down()
        {
        }
    }
}