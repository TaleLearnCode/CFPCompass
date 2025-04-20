MERGE dbo.ShindigStatus AS TARGET
USING (VALUES ( 1, 1, 1, 'Upcoming CFP',          'The event exists, but the CFP has not opened yet.'),
              ( 2, 2, 1, 'Open CFP',              'The CFP is currently accepting submissions.'),
              ( 3, 3, 1, 'Closing Soon',          'The CFP is nearing its deadline.'),
              ( 4, 4, 1, 'Closed CFP',            'The submission deadline has passed.'),
              ( 5, 5, 1, 'Selection in Progress', 'Speakers are being reviewed/selected.'),
              ( 6, 6, 1, 'Speakers Annoucned',    'The lineup has been finalized.'),
              ( 7, 7, 1, 'Event Scheduled',       'The CFP phase is done, and the event is officially scheduled.'),
              ( 8, 8, 1, 'Event Completed',       'The event has taken place'),
              ( 9, 9, 1, 'Canceled',              'The event or CFP was called of.'))
AS SOURCE (ShindigStatusId,
           SortOrder,
           IsEnabled,
           ShindigStatusName,
           ShindigStatusDescription)
ON TARGET.ShindigStatusId = SOURCE.ShindigStatusId
WHEN MATCHED THEN UPDATE SET TARGET.ShindigStatusName        = SOURCE.ShindigStatusName,
                             TARGET.ShindigStatusDescription = SOURCE.ShindigStatusDescription,
                             TARGET.SortOrder               = SOURCE.SortOrder,
                             TARGET.IsEnabled               = SOURCE.IsEnabled
WHEN NOT MATCHED THEN INSERT (ShindigStatusId,
                              ShindigStatusName,
                              ShindigStatusDescription,
                              SortOrder,
                              IsEnabled)
                      VALUES (SOURCE.ShindigStatusId,
                              SOURCE.ShindigStatusName,
                              SOURCE.ShindigStatusDescription,
                              SOURCE.SortOrder,
                              SOURCE.IsEnabled)
WHEN NOT MATCHED BY SOURCE THEN DELETE;
GO