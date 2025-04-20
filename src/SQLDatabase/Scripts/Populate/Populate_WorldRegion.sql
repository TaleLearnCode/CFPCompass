MERGE INTO dbo.WorldRegion AS TARGET
USING (VALUES 

              -- World
              ('001', 1, NULL, 'World'),
              
              -- Africa
              ('002', 1, '001', 'Africa'),
              ('015', 1, '002', 'Northern Africa'),
              ('202', 1, '002', 'Sub-Saharan Africa'),
                ('011', 1, '202', 'Western Africa'),
                ('014', 1, '202', 'Eastern Africa'),
                ('017', 1, '202', 'Middle Africa'),
                ('018', 1, '202', 'Southern Africa'),

              -- Antarctica
              ('010', 1, '001', 'Antarctica'),

              -- Americas
              ('019', 1, '001', 'Americas'),
              ('003', 1, '019', 'North America'),
                ('021', 1, '003', 'Northern America'),
              ('419', 1, '019', 'Latin America and the Caribbean'),
                ('005', 1, '419', 'South America'),
                ('013', 1, '419', 'Central America'),
                ('029', 1, '419', 'Caribbean'),

              -- Asia
              ('142', 1, '001', 'Asia'),
              ('030', 1, '142', 'Eastern Asia'),
              ('034', 1, '142', 'Southern Asia'),
              ('035', 1, '142', 'South-Eastern Asia'),
              ('143', 1, '142', 'Central Asia'),
              ('145', 1, '142', 'Western Asia'),

              -- Europe
              ('150', 1, '001', 'Europe'),
              ('039', 1, '150', 'Southern Europe'),
              ('151', 1, '150', 'Eastern Europe'),
              ('154', 1, '150', 'Northern Europe'),
                ('830', 1, '154', 'Channel Islands'),
              ('155', 1, '150', 'Western Europe'),

              -- Oceania
              ('009', 1, '001', 'Oceania'),
              ('053', 1, '009', 'Australia and New Zealand'),
              ('054', 1, '009', 'Melanesia'),
              ('057', 1, '009', 'Micronesia'),
              ('061', 1, '009', 'Polynesia'))

AS SOURCE (WorldRegionCode,
           IsEnabled,
           ParentId,
           WorldRegionName)
ON TARGET.WorldRegionCode = SOURCE.WorldRegionCode
WHEN MATCHED THEN UPDATE SET TARGET.WorldRegionName = SOURCE.WorldRegionName,
                             TARGET.ParentId        = SOURCE.ParentId,
                             TARGET.IsEnabled       = SOURCE.IsEnabled
WHEN NOT MATCHED THEN INSERT (WorldRegionCode,
                              WorldRegionName,
                              ParentId,
                              IsEnabled)
                      VALUES (SOURCE.WorldRegionCode,
                              SOURCE.WorldRegionName,
                              SOURCE.ParentId,
                              SOURCE.IsEnabled);