CREATE TABLE [dbo].[AccountTransaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [int] NOT NULL,
	[Date] DATETIME2 CONSTRAINT [DF_AccountTransaction_Date] DEFAULT (getutcdate()) NOT NULL,
	[Description] [varchar](200) NOT NULL,
	[TransactionType] [varchar](10) NOT NULL,
	[Amount] [decimal](19,2) NOT NULL,
	[CurrentBalance] [decimal](19,2) NOT NULL
	CONSTRAINT [PK_AccountTransaction] PRIMARY KEY CLUSTERED ([TransactionId] ASC),
	CONSTRAINT [FK_AccountTransaction_AccountNumber] FOREIGN KEY ([AccountNumber]) REFERENCES [dbo].[AccountSummary] ([AccountNumber]) 
 );
