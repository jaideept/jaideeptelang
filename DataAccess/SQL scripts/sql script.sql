CREATE TABLE Contact (
	Id int NOT NULL
		IDENTITY(1,1)
		PRIMARY KEY NONCLUSTERED,
      FirstName varchar(150) NOT NULL,
      LastName varchar(150) NOT NULL,
	  Email varchar(150) NOT NULL,
	  Phone varchar(150) NOT NULL,
	  Status bit NOT NULL DEFAULT 1
)