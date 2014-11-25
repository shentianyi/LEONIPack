USE [Leoni_Packaging_prod]
GO

/****** Object:  Table [dbo].[Trays]    Script Date: 11/25/2014 18:48:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Trays](
	[trayId] [varchar](30) NOT NULL,
	[createTime] [datetime] NOT NULL,
	[warehouse] [varchar](30) NOT NULL,
	[position] [varchar](30) NOT NULL,
	[status] [int] NOT NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[sync] [bit] NULL,
 CONSTRAINT [PK_Trays] PRIMARY KEY CLUSTERED 
(
	[trayId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Trays] ADD  CONSTRAINT [MSmerge_df_rowguid_9729D565EB7946BD9CFEDD2A5C1B2631]  DEFAULT (newsequentialid()) FOR [rowguid]
GO

ALTER TABLE [dbo].[Trays] ADD  CONSTRAINT [DF_Trays_sync]  DEFAULT ((0)) FOR [sync]
GO


