CREATE TABLE dbo.ShindigType
(
  ShindigTypeId          INT           NOT NULL,
  ShindigTypeName        NVARCHAR(100) NOT NULL,
  ShindigTypeDescription NVARCHAR(500)     NULL,
  SortOrder              INT           NOT NULL,
  IsEnabled              BIT           NOT NULL,
  CONSTRAINT pkcShindigType PRIMARY KEY (ShindigTypeId)
)
GO

EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ShindigType',                                        @value=N'Represents a type of a shindig.',                                                   @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ShindigType', @level2name=N'ShindigTypeId',          @value=N'The identifier of the shindig type record.',                                        @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ShindigType', @level2name=N'ShindigTypeName',        @value=N'The name of the shindig type.',                                                     @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ShindigType', @level2name=N'ShindigTypeDescription', @value=N'A description of the shindig type.',                                                @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ShindigType', @level2name=N'SortOrder',              @value=N'The sorting order of the shindig type.',                                            @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ShindigType', @level2name=N'IsEnabled',              @value=N'Flag indicating whether the shindig type is enabled.',                              @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'ShindigType', @level2name=N'pkcShindigType',         @value=N'Defines the primary key for the ShindigType table using the ShindigTypeId column.', @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'CONSTRAINT';
GO