﻿using FluentMigrator;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201601291443)]
    public class _201601291443_InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table(DbConstants.Tables.ItemTable.TableName)
                .WithColumn(DbConstants.Tables.ItemTable.Columns.ItemId).AsGuid().PrimaryKey()
                .WithColumn(DbConstants.Tables.ItemTable.Columns.ItemName).AsString(512).NotNullable()
                .WithColumn(DbConstants.Tables.PersonTable.Columns.PersonId).AsGuid().ForeignKey();

            Create.Table(DbConstants.Tables.PersonTable.TableName)
                .WithColumn(DbConstants.Tables.PersonTable.Columns.PersonId).AsGuid().PrimaryKey()
                .WithColumn(DbConstants.Tables.PersonTable.Columns.FirstName).AsString(500)
                .WithColumn(DbConstants.Tables.PersonTable.Columns.Email).AsString(500)
                .WithColumn(DbConstants.Tables.PersonTable.Columns.LastName).AsString(500);
        }

        public override void Down()
        {
        }
    }
}