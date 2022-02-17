SET IDENTITY_INSERT BookDB.dbo.Copies ON

INSERT INTO BookDB.dbo.Copies(CopyId, CopyType, BookId, BookReservationId)
VALUES (0,1,1,NULL),(1,2,1,NULL),(2,1,2,NULL),(3,2,2,NULL),(4,1,3,NULL),(5,2,3,NULL),(6,1,4,NULL),(7,2,4,NULL);

SET IDENTITY_INSERT BookDB.dbo.Copies OFF
