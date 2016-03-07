namespace LendingLibrary.DB
{
    public class DbConstants
    {
        public class Tables
        {
            public class ItemTable
            {
                public const string TableName = "Item";
                public class Columns
                {
                    public const string ItemId = "ItemId";
                    public const string ItemName = "ItemName";
                    public const string Photo = "Photo";
                    public const string PersonId = "PersonId";
                }
            }

            public class PersonTable
            {
                public const string TableName = "Person";
                public class Columns
                {
                    public const string PersonId = "PersonId";
                    public const string FirstName = "FirstName";
                    public const string LastName = "LastName";
                    public const string Email = "Email";
                    public const string PhoneNumber = "PhoneNumber";
                    public const string Title = "Title";
                }
            }

            public class Common
            {
                public class Columns
                {
                    public const string DateCreated = "DateCreated";
                    public const string CreatedUsername = "CreatedUsername";
                    public const string DateLastModified = "DateLastModified";
                    public const string LastModifiedUsername = "LastModifiedUsername";

                }
            }
        }
    }
}