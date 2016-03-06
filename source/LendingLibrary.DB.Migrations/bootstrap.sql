-- bootstrap script for LendingLibrary application
-- Please run before attempting to start the application
------ uncomment next few lines if you'd *really* like to recreate the database
 --use master;
 --go
 --ALTER DATABASE  [LendingLibrary]
 --SET SINGLE_USER
 --WITH ROLLBACK IMMEDIATE
 --drop database [LendingLibrary]
 --go
------ normal creation after here
declare @spid int;
select @spid = min(spid) from master.dbo.sysprocesses where dbid = db_id('LendingLibrary');
while @spid is not null
    begin
        exec('Kill '+@spid);
        select @spid = min(spid) from master.dbo.sysprocesses where dbid = db_id('LendingLibrary');
    end;
go
create database LendingLibrary;
go
use LendingLibrary;
go
if not exists (select name from master..syslogins where name = 'LendingLibraryOwner')
    begin
        create login [LendingLibraryOwner] with password = '5h4cl3+0nW3b';
    end;
go
create user [LendingLibraryOwner]
	for login [LendingLibraryOwner]
	with default_schema = dbo
GO
grant connect to [LendingLibrary]
go
exec sp_addrolemember N'db_datareader', N'LendingLibraryOwner';
go
exec sp_addrolemember N'db_datawriter', N'LendingLibraryOwner';
go
exec sp_addrolemember N'db_owner', N'LendingLibraryOwner';
go


