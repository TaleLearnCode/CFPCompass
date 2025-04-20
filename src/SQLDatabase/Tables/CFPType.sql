CREATE TABLE dbo.CFPType
(
  CFPTypeId          INT           NOT NULL,
  CFPTypeName        NVARCHAR(100) NOT NULL,
  CFPTypeDescription NVARCHAR(500)     NULL,
  SortOrder          INT           NOT NULL,
  IsEnabled          BIT           NOT NULL,
  CONSTRAINT pkcCFPType PRIMARY KEY (CFPTypeId)
)
GO