Use [GridProject]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE usp_GetFileList
@email nvarchar(300)
AS
BEGIN
	select *
	from tblFileMetaData
	where Email_User = @email
END
GO
