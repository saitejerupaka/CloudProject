Use [GridProject]

Create Table tblUserInfo( 
						  UserEmail nvarchar(300) NOT NULL,
                          FirstName nvarchar(100) NOT NULL,
                          LastName nvarchar(100) NOT NULL,
                          [PassWord] nvarchar(100) Not Null,
                          BucketName nvarchar(300) NOT NULL,
                          Primary key (UserEmail)
                        )