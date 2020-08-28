USE [SessionDatabase]
GO

CREATE PROCEDURE [dbo].[SaveToDatabase_StoredProcedure]
	@SessionId nvarchar(50), 
	@Status nvarchar(max),
	@UserAdress nvarchar(max),
	@IdVideo nvarchar(max),
	@RequestTime int
AS
BEGIN TRY
	BEGIN TRANSACTION
	INSERT INTO Session VALUES (@SessionId, @Status, @UserAdress, @IdVideo, @RequestTime);
	IF NOT EXISTS (SELECT IdVideo FROM VideoPlayers WHERE IdVideo = @IdVideo)
	INSERT INTO VideoPlayers VALUES (@IdVideo);
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
END CATCH