Use [GridProject]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE usp_LoginUser
@email nvarchar(300),
@pwd nvarchar(10)
AS
BEGIN
	select UserEmail,FirstName,LastName,BucketName from tblUserInfo 
	where UserEmail = @email
	and 
	[PassWord] = @pwd
END
GO
