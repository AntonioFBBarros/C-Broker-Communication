CREATE TABLE [dbo].[Users] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Email]        VARCHAR (255) NOT NULL,
    [PasswordHash] VARCHAR (255) NOT NULL,
    [Valid]        INT           DEFAULT ((0)) NOT NULL
);



CREATE TABLE [dbo].[SensorInfo] (
    [Id]        INT           NOT NULL,
    [Floor]     INT           NOT NULL,
    [Location]  VARCHAR (MAX) NOT NULL,
    [UserId]    INT           NULL,
    [Latitude]  VARCHAR (255) NULL,
    [Longitude] VARCHAR (255) NULL
);


CREATE TABLE [dbo].[SensorData] (
    [Id]          INT        IDENTITY (1, 1) NOT NULL,
    [SensorId]    INT        NOT NULL,
    [Temperature] FLOAT (53) NULL,
    [Humidity]    FLOAT (53) NULL,
    [TimeStamp]   DATETIME   NULL,
    [Valid]       INT        DEFAULT ((1)) NOT NULL,
    [Battery]     INT        NULL
);



CREATE TABLE [dbo].[Alarms] (
    [SensorDataId] INT           NOT NULL,
    [Reason]       VARCHAR (MAX) NOT NULL
);


SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([Id], [Email], [PasswordHash], [Valid]) VALUES (6, N'andre@my.ipleiria.pt', N'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', 1)
INSERT INTO [dbo].[Users] ([Id], [Email], [PasswordHash], [Valid]) VALUES (7, N'andre1212@my.ipleiria.pt', N'ungWv48Bz+pBQUDeXa4iI7ADYaOWF3qctBD/YfIAFa0=', 0)
SET IDENTITY_INSERT [dbo].[Users] OFF


INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (1, 1, N'Biblioteca', NULL, N'39.733296', N'-8.820811')
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (2, 2, N'Biblioteca', NULL, NULL, NULL)
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (3, 1, N'EdificioA', NULL, N'39.732612', N'-8.820501')
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (4, 2, N'EdificioA', NULL, NULL, NULL)
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (5, 1, N'Biblioteca', NULL, NULL, NULL)
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (6, 3, N'Biblioteca', 6, NULL, NULL)
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (7, 3, N'Biblioteca', 6, NULL, NULL)
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (8, 4, N'Biblioteca', 6, N'39.733296', N'-8.820811')
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (9, 5, N'Biblioteca', 6, NULL, NULL)
INSERT INTO [dbo].[SensorInfo] ([Id], [Floor], [Location], [UserId], [Latitude], [Longitude]) VALUES (12, 1, N'Biblioteca', 6, N'39.733296', N'-8.820811')

SET IDENTITY_INSERT [dbo].[SensorData] ON
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (5, 3, 10.5, 40.5, N'2019-12-12 00:00:00', 1, 100)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (6, 3, 10.5, 40.5, N'2019-12-12 14:00:00', 1, 70)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (7, 3, 10.5, 40.5, N'2019-12-12 14:00:00', 1, 80)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (9, 3, 10.5, 40.5, N'2019-12-12 17:00:00', 1, 25)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (10, 3, 10.5, 40.5, N'2019-12-13 18:00:00', 1, NULL)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (11, 2, 19.6, 40.5, N'2019-12-13 18:00:00', 1, NULL)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (12, 2, 22.5, 57.5, N'2019-09-30 00:09:00', 1, NULL)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (13, 4, 22.5, 57.5, N'2019-09-30 00:09:00', 1, NULL)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (14, 1, 22.5, 57.5, N'2019-09-30 00:09:00', 1, NULL)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (15, 4, 20, 50, N'2019-02-12 00:09:00', 1, NULL)
INSERT INTO [dbo].[SensorData] ([Id], [SensorId], [Temperature], [Humidity], [TimeStamp], [Valid], [Battery]) VALUES (16, 4, 20, 50, N'2019-11-12 00:09:00', 1, NULL)
SET IDENTITY_INSERT [dbo].[SensorData] OFF

INSERT INTO [dbo].[Alarms] ([SensorDataId], [Reason]) VALUES (5, N'Reason')