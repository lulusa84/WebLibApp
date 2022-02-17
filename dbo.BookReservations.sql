CREATE TABLE [dbo].[BookReservations] (
    [BookReservationId]  INT            IDENTITY (1, 1) NOT NULL,
    [AppUserId]          NVARCHAR (450) NOT NULL,
    [ReservationDate]    DATETIME2 (7)  NOT NULL,
    [ReservationEndDate] DATETIME2 (7)  NULL,
    [CopyId]             INT            NOT NULL,
    CONSTRAINT [PK_BookReservations] PRIMARY KEY CLUSTERED ([BookReservationId] ASC),
    CONSTRAINT [FK_BookReservations_AspNetUsers_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_BookReservations_Copies_CopyId] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[Copies] ([CopyId])
);


GO
CREATE NONCLUSTERED INDEX [IX_BookReservations_AppUserId]
    ON [dbo].[BookReservations]([AppUserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BookReservations_CopyId]
    ON [dbo].[BookReservations]([CopyId] ASC);

