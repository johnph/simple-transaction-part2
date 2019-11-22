CREATE TABLE [dbo].[AccountSummary](
	[AccountNumber] [int] NOT NULL,
	[Balance] [decimal](19,2) NOT NULL,
	[Currency] [varchar](3) NOT NULL
	CONSTRAINT [PK_AccountSummary] PRIMARY KEY CLUSTERED([AccountNumber])
 );
