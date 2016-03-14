using FluentMigrator;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201603141701)]
    public class _201603141701_CreateForeignKey : Migration
    {
        public override void Up()
        {
            Create.ForeignKey() // You can give the FK a name or just let Fluent Migrator default to one
            .FromTable("Lending").ForeignColumn("PersonId")
            .ToTable("Person").PrimaryColumn("PersonId");

            Create.ForeignKey() // You can give the FK a name or just let Fluent Migrator default to one
            .FromTable("Lending").ForeignColumn("ItemId")
            .ToTable("Item").PrimaryColumn("ItemId");
        }

        public override void Down()
        {
        }
    }
}
