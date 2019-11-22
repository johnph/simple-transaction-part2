/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
Insert Into dbo.AccountSummary(AccountNumber, Balance, Currency) Values(3628101, 25000, 'EUR')
Insert Into dbo.AccountSummary(AccountNumber, Balance, Currency) Values(3637897, 75000, 'EUR')
Insert Into dbo.AccountSummary(AccountNumber, Balance, Currency) Values(3648755, 117600, 'EUR')
