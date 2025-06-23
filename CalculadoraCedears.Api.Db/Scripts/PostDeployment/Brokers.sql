/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

SET NOCOUNT ON

MERGE INTO [Brokers] AS [Target]
USING (VALUES 
(1,'Bull Market', 0.70)) AS [Source] ([Id],[Name],[Comision])
ON ([Target].[Id] = [Source].[Id])
WHEN MATCHED AND (
    NULLIF([Source].[Name], [Target].[Name]) IS NOT NULL OR NULLIF([Target].[Name],[Source].[Name]) IS NOT NULL
	OR
	NULLIF([Source].[Comision], [Target].[Comision]) IS NOT NULL OR NULLIF([Target].[Comision],[Source].[Comision]) IS NOT NULL)
THEN
UPDATE SET
   [Name] = [Source].[Name],
   [Comision] = [Source].[Comision]
WHEN NOT MATCHED BY TARGET THEN
INSERT([Id],[Name],[Comision])
VALUES([Source].[Id],[Source].[Name],[Source].[Comision]) WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
GO

DECLARE @mergeError int,
	@mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT

IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [Brokers]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100));
 END
ELSE
 BEGIN
 PRINT '[Brokers] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO

SET NOCOUNT OFF
GO