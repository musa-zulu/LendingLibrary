﻿using System.Data.Entity.ModelConfiguration;
using LendingLibrary.Core.Domain;
using ItemTable = LendingLibrary.DB.DbConstants.Tables.ItemTable;

namespace LendingLibrary.DB.Mapping
{
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            HasKey(it => it.Id);

            ToTable(ItemTable.TableName);
            Property(p => p.Id).HasColumnName(ItemTable.Columns.ItemId);
            Property(p => p.ItemName).HasColumnName(ItemTable.Columns.ItemName);
            Property(p => p.Photo).HasColumnName(ItemTable.Columns.Photo);
        }
    }
}