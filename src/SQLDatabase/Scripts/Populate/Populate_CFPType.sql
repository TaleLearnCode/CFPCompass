MERGE dbo.CFPType AS TARGET
USING (VALUES ( 1, 1, 1, 'Sessionize', 'The CFP is executed on the Sessionize platform.'),
              ( 2, 2, 1, 'PaperCall',  'The CFP is executed on the PaperCall platform.'),
              ( 3, 3, 1, 'Custom',     'The CFP is using a custom platform.'))
AS SOURCE (CFPTypeId,
           SortOrder,
           IsEnabled,
           CFPTypeName,
           CFPTypeDescription)
ON TARGET.CFPTypeId = SOURCE.CFPTypeId
WHEN MATCHED THEN UPDATE SET TARGET.CFPTypeName        = SOURCE.CFPTypeName,
                             TARGET.CFPTypeDescription = SOURCE.CFPTypeDescription,
                             TARGET.SortOrder          = SOURCE.SortOrder,
                             TARGET.IsEnabled          = SOURCE.IsEnabled
WHEN NOT MATCHED THEN INSERT (CFPTypeId,
                              CFPTypeName,
                              CFPTypeDescription,
                              SortOrder,
                              IsEnabled)
                      VALUES (SOURCE.CFPTypeId,
                              SOURCE.CFPTypeName,
                              SOURCE.CFPTypeDescription,
                              SOURCE.SortOrder,
                              SOURCE.IsEnabled)
WHEN NOT MATCHED BY SOURCE THEN DELETE;
GO