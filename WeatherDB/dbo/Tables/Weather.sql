CREATE TABLE [dbo].[Weather]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DateTime] DATETIME2 NOT NULL , 
    [Temperature] FLOAT NOT NULL, 
    [Humidity] FLOAT NOT NULL, 
    [Pressure] FLOAT NOT NULL, 
    [Wind] FLOAT NOT NULL
)
