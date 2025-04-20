MERGE dbo.ShindigType AS TARGET
USING (VALUES ( 1, 1, 1, 'Conference',    'A large formal gathering where industry professionals discuss advancements, share knowledge, and network through keynotes, panels, and sessions.'),
              ( 2, 2, 1, 'User Group',    'A community-driven meeting where users of a particular technology or service share experiences, learn, and hear from experts in a collaborative environment.'),
              ( 3, 3, 1, 'Code Camp',     'An informal, community-organized event emphasizing hands-on learning and collaboration through intensive coding sessions, workshops, and presentations.'),
              ( 4, 4, 1, 'Interview',     'A one-on-one or panel interaction where a speaker shares expertise, experiences, and insights on specific topics in a structured Q&A format.'),
              ( 5, 5, 1, 'Workshop',      'An interactive session focused on teaching specific skills or techniques through practical, hands-on activities guided by an expert instructor.'),
              ( 6, 6, 1, 'Private Event', 'An exclusive, invitation-only gathering tailored to a specific audience, offering presentations, discussions, and networking in an intimate setting.'))
AS SOURCE (ShindigTypeId,
           SortOrder,
           IsEnabled,
           ShindigTypeName,
           ShindigTypeDescription)
ON TARGET.ShindigTypeId = SOURCE.ShindigTypeId
WHEN MATCHED THEN UPDATE SET TARGET.ShindigTypeName        = SOURCE.ShindigTypeName,
                             TARGET.ShindigTypeDescription = SOURCE.ShindigTypeDescription,
                             TARGET.SortOrder               = SOURCE.SortOrder,
                             TARGET.IsEnabled               = SOURCE.IsEnabled
WHEN NOT MATCHED THEN INSERT (ShindigTypeId,
                              ShindigTypeName,
                              ShindigTypeDescription,
                              SortOrder,
                              IsEnabled)
                      VALUES (SOURCE.ShindigTypeId,
                              SOURCE.ShindigTypeName,
                              SOURCE.ShindigTypeDescription,
                              SOURCE.SortOrder,
                              SOURCE.IsEnabled)
WHEN NOT MATCHED BY SOURCE THEN DELETE;
GO