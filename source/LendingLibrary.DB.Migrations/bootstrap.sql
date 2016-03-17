-- bootstrap script for LendingLibrary application
-- Please run before attempting to start the application
------ uncomment next few lines if you'd *really* like to recreate the database
-- use master;
-- go
-- ALTER DATABASE  [LendingLibrary]
-- SET SINGLE_USER
-- WITH ROLLBACK IMMEDIATE
-- drop database [LendingLibrary]
-- go
------ normal creation after here
use master;
go
if not exists (select name from master..syslogins where name = 'LendingLibrary')
    begin
        create login LendingLibrary with password = 'sa';
    end;
go


if not exists (select name from master..sysdatabases where name = 'LendingLibrary')
begin
create database LendingLibrary
end;
GO

use LendingLibrary
if not exists (select * from sysusers where name = 'LendingLibrary')
begin
create user LendingLibrary
	for login LendingLibrary
	with default_schema = dbo
end;
GO
grant connect to LendingLibrary
go
exec sp_addrolemember N'db_datareader', N'LendingLibrary';
go
exec sp_addrolemember N'db_datawriter', N'LendingLibrary';
go
exec sp_addrolemember N'db_owner', N'LendingLibrary';
GO
use master;
GO

