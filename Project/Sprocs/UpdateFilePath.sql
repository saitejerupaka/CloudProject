Use [GridProject]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE usp_UpdateFilePath
@FileID int,
@FilePath nvarchar(500),
@FileName nvarchar(300)
AS
BEGIN
	Update tblFileMetaData 
	set
	FilePath = @FilePath,
	[FileName] = @FileName
	where 
	FileID = @FileID
END
GO
