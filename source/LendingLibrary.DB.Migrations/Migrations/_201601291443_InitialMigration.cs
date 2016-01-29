using FluentMigrator;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201601291443)]
    public class _201601291443_InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table(DbConstants.Tables.ItemTable.TableName)
                .WithColumn(DbConstants.Tables.ItemTable.Columns.ItemId).AsGuid().PrimaryKey()
                .WithColumn(DbConstants.Tables.ItemTable.Columns.ItemName).AsString(512).NotNullable();
        }

        public override void Down()
        {
        }
    }
}