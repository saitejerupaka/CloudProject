Use [GridProject]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create Procedure usp_UpdateLastModified
@file_ID int
As
Begin
	Update tblFileMetaData
	set LastModified = CURRENT_TIMESTAMP
	where FileID = @file_ID
END
GO
