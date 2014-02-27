Use [GridProject]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE usp_DeleteFile
@file_ID int
AS
BEGIN
	Delete from tblFileMetaData
	where FileID = @file_ID
END
GO
