﻿using System.Data.Entity.ModelConfiguration;
using LendingLibrary.Core.Domain;
using PersonTable = LendingLibrary.DB.DbConstants.Tables.PersonTable;
namespace LendingLibrary.DB.Mapping
{
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            HasKey(it => it.PersonId);

            ToTable(PersonTable.TableName);
            Property(p => p.PersonId).HasColumnName(PersonTable.Columns.PersonId);
            Property(p => p.FirstName).HasColumnName(PersonTable.Columns.FirstName);
            Property(p => p.LastName).HasColumnName(PersonTable.Columns.LastName);
            Property(p => p.Email).HasColumnName(PersonTable.Columns.Email);
            Property(p => p.PhoneNumber).HasColumnName(PersonTable.Columns.PhoneNumber);
        }
    }
}