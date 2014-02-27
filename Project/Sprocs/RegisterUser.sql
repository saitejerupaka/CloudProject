use [GridProject]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE usp_RegisterUser
@email nvarchar(300),
@fname nvarchar(100),
@lname nvarchar(100),
@pwd nvarchar(10),
@bucketname nvarchar(100)
AS
BEGIN
	insert into tblUserInfo (UserEmail,FirstName,LastName,[PassWord],BucketName)
	values(@email,@fname,@lname,@pwd,@bucketname)
END
GO
