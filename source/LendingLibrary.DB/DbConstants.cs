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
                    public const string ItemDescription = "ItemDescription";
                    public const string Mimetype = "Mimetype";
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

            public class LendingTable
            {
                public const string TableName = "Lending";
                public class Columns
                {
                    public const string LedingId = "LedingId";
                    public const string PersonId = "PersonId";
                    public const string ItemId = "ItemId";
                    public const string DateBorrowed = "DateBorrowed";
                    public const string DateReturned = "DateReturned";
                    public const string LendingStatus = "LendingStatus";
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