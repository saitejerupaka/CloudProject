Use [GridProject]	

Create Table tblFileMetaData(
								FileID int identity(1,1) Not Null,
								[FileName] nvarchar(300) Not Null,
								Email_User nvarchar(300) Not Null,
								FilePath nvarchar(500) Not Null,
								LastModified DateTime Not Null,
								Primary Key (FileID),
								Foreign Key (Email_User) references tblUserInfo(UserEmail)
							)