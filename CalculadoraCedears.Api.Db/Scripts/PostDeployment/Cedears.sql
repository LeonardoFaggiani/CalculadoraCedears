﻿/*
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

MERGE INTO [Cedears] AS [Target]
USING (VALUES 
('B7FC6D40-E893-4A28-A482-F65981077CB0','AAPL','Apple Inc.',10),
('6A19AE3D-4367-4647-B5E0-5ED260FF2D5D','MSFT','Microsoft Corp.',30),
('E31291D5-4DC5-457B-ACEB-0299DC5AC883','TSLA','Tesla Inc.',15),
('1EF2A417-1E79-4E7A-A505-12C9FAAA0771','META','Meta Platforms Inc.',24),
('87EF9DD0-C224-4244-A21D-5615347C553C','AMD','Advanced Micro Devices',10),
('4A055DE2-EBD4-4312-BD3C-BCA7DB7D032A','AMZN','Amazon.com Inc.',144),
('B0869F60-8CA5-4587-AAB5-F4A78FA20AB7','GOOGL','Alphabet Inc.',28),
('E0158706-423E-42B1-B634-8D3701E33BF3','NFLX','Netflix Inc.',18),
('1A613119-1C6B-46A1-9F29-16B89E49751E','NVDA','NVIDIA Corp.',30),
('31278117-87BD-4CD9-A2DA-44857EE31645','JPM','JPMorgan Chase & Co.',5),
('748A251C-EB9C-4A74-B17C-3CBB45DEF302','JNJ','Johnson & Johnson',15),
('BC53ADC1-B0D4-4D22-8EA5-F31B807D89CC','DIS','The Walt Disney Company',12),
('6998CAC1-63F3-4A24-8A48-498AD4F2ADBE','MCD','McDonalds Corp.',24),
('81E2885A-C380-4543-A5FD-6E88603AFB85','PFE','Pfizer Inc.',2),
('AF8ABAD8-E862-463F-8098-DEF0A087DFFE','KO','Coca-Cola Co.',5),
('CB5D9D80-E1DA-4718-826E-3BBAC1101947','PG','Procter & Gamble Co.',5),
('619840D8-BEE5-4CA1-98DD-CA87D48D19B7','TGT','Target Corp.',24),
('196BDC10-E77C-4902-9A1A-7DFD4F472121','V','Visa Inc.',18),
('FDF59B48-AF59-45C5-92AA-1909BB6838AC','SQ','Block Inc.',20),
('8E28EE38-6F90-4AAA-B7CB-F566656FCE9D','SHOP','Shopify Inc.',107),
('0A258BC7-2455-4E84-A962-E33C7DDBBC29','SNOW','Snowflake Inc.',30),
('F7F1DB43-367C-4CFB-A6FB-54FDFFBC164F','SPOT','Spotify Technology S.A.',28),
('603AB4D9-E364-4941-8E01-7F251536FBB2','SAP','SAP SE',6),
('67FF733F-1F49-4944-8F48-E89CA416B399','SBUX','Starbucks Corp.',12),
('3ACDFBC6-A00B-4C83-95D9-FA0AC948A9E2','PEP','PepsiCo Inc.',6),
('BBF28420-A01F-4F95-892B-22C8B7B07843','PYPL','PayPal Holdings Inc.',8),
('482A4629-E100-4F15-B728-AE3860F959B7','TSM','Taiwan Semiconductor Manufacturing Co.',9),
('9171E49F-509B-4701-8BC1-AC01A9745863','CAT','Caterpillar Inc.',20),
('BE14B5C1-C447-4EE4-BE92-766BF76F59F1','GE','General Electric Co.',8),
('7913AF99-5AED-4D8F-AEF1-9FCA1E96851D','COST','Costco Wholesale Corp.',48),
('554FAF1A-7237-4008-A8E1-AAB2603BE306','JMIA','Jumia Technologies AG',1)
) AS [Source] ([Id],[Ticker],[Name],[Ratio])
ON ([Target].[Id] = [Source].[Id])
WHEN MATCHED AND (
    NULLIF([Source].[Ticker], [Target].[Ticker]) IS NOT NULL OR NULLIF([Target].[Ticker],[Source].[Ticker]) IS NOT NULL) THEN
UPDATE SET
   [Ticker] = [Source].[Ticker],
   [Name] = [Source].[Name],
   [Ratio] = [Source].[Ratio]
WHEN NOT MATCHED BY TARGET THEN
INSERT([Id],[Ticker],[Name],[Ratio])
VALUES([Source].[Id],[Source].[Ticker],[Source].[Name],[Source].[Ratio]) WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
GO

DECLARE @mergeError int,
	@mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT

IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [Cedears]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100));
 END
ELSE
 BEGIN
 PRINT '[Cedears] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO

SET NOCOUNT OFF
GO