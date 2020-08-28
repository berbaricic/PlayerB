USE SessionDatabase;
GO

CREATE PROCEDURE SaveToDatabase_StoredProcedure
	-- Add the parameters for the stored procedure here
	@SessionId nvarchar(50), 
	@Status nvarchar(max),
	@UserAdress nvarchar(max),
	@IdVideo nvarchar(max),
	@RequestTime int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Session VALUES (@SessionId, @Status, @UserAdress, @IdVideo, @RequestTime);
END
GO
