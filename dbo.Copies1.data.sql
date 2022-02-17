UPDATE [dbo].[Copies]
SET [dbo].[Copies].CopyId = 4
WHERE CopyType = 0 AND [dbo].[Copies].Bookid = 2;

UPDATE [dbo].[Copies]
SET [dbo].[Copies].CopyId = 5
WHERE CopyType = 1 AND [dbo].[Copies].Bookid = 2;

UPDATE [dbo].[Copies]
SET [dbo].[Copies].CopyId = 6
WHERE CopyType = 0 AND [dbo].[Copies].Bookid = 3;

UPDATE [dbo].[Copies] 
SET [dbo].[Copies].CopyId = 7
WHERE CopyType = 1 AND [dbo].[Copies].Bookid = 3;

UPDATE [dbo].[Copies]
SET [dbo].[Copies].CopyId = 9
WHERE CopyType = 0 AND [dbo].[Copies].Bookid = 4;

UPDATE [dbo].[Copies] 
SET [dbo].[Copies].CopyId = 10
WHERE CopyType = 1 AND [dbo].[Copies].Bookid = 4;