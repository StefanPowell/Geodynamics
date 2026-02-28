DROP PROCEDURE [dbo].[InsertFaultLines];

ALTER TABLE [dbo].[faultStress] 
DROP CONSTRAINT [FK_faultStress_faultId];
GO

DROP TABLE [dbo].[faultStress];

DROP TYPE [dbo].[FaultLine];

DROP TABLE [dbo].[faultCoordinates];

DROP TABLE [dbo].[fault];

Go
