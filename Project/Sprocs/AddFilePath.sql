Use [GridProject]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE usp_AddFilePath
	@email nvarchar(300),
	@filepath nvarchar(500),
	@filename nvarchar(300)
	
AS
BEGIN
	insert into tblFileMetaData( [FileName], Email_User, FilePath, LastModified)
	values(@filename, @email, @filepath, CURRENT_TIMESTAMP)
END
GO
