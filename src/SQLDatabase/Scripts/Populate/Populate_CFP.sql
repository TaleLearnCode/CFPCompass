MERGE dbo.CFP AS TARGET
USING (VALUES ('apicon-new-york-2025',                           3, '2025-03-22', '2025-04-22', 1, 1, 1, 'https://apiconference.net/new-york/call-for-papers-ny/'),
              ('devopscon-new-york-2025',                        3, '2025-03-22', '2025-04-22', 1, 1, 1, 'https://devopscon.io/call-for-papers-new-york/'),
              ('techoroma-netherlands-2025',                     1, '2025-02-17', '2025-04-25', 1, 1, 1, 'https://sessionize.com/techorama-2025-netherlands/'),
              ('prairie-dev-con-winnipeg-2025',                  2, '2025-04-03', '2025-05-09', 1, 1, 1, 'https://www.papercall.io/prdcwinnipeg2025'),
              ('ncd-porto-2025',                                 1, '2025-01-22', '2025-05-18', 1, 1, 1, 'https://sessionize.com/ndc-porto-2025/'),
              ('apicon-berlin-2025',                             3, '2025-04-01', '2025-05-19', 1, 1, 1, 'https://callforpapers.sandsmedia.com/'),
              ('serverless-architecture-conference-berlin-2025', 3, '2025-04-01', '2025-05-19', 1, 1, 1, 'https://callforpapers.sandsmedia.com/'),
              ('build-stuff-2025',                               1, '2025-03-01', '2025-06-30', 1, 1, 1, 'https://sessionize.com/build-stuff-2025'),
              ('atlanta-cloud-conference-2025',                  1, '2025-02-01', '2025-03-23', 1, 1, 1, 'https://sessionize.com/atlanta-cloud-conference-2025/'),
              ('stir-trek-2025',                                 1, '2025-01-14', '2025-01-28', 1, 1, 1, 'https://sessionize.com/stir-trek-2025/'),
              ('devup-2025',                                     1, '2025-01-13', '2025-02-28', 1, 1, 1, 'https://sessionize.com/dev-up-2025/'),
              ('beer-city-code-2025',                            1, '2025-01-05', '2025-03-01', 1, 1, 1, 'https://sessionize.com/beer-city-code-2025/'),
              ('kcdc-2025',                                      1, '2025-01-07', '2025-02-21', 1, 1, 1, 'https://sessionize.com/kcdc-2025/'),
              ('devconf-2025',                                   1, '2025-03-17', '2025-05-17', 1, 1, 1, 'https://sessionize.com/devconfpl-2025'),
              ('cloudbrew-2025',                                 1, '2025-04-22', '2025-06-08', 1, 1, 1, 'https://sessionize.com/cloudbrew-2025'),
              ('dotnet-developer-conference-25',                 1, '2025-04-15', '2025-06-01', 1, 1, 1, 'https://sessionize.com/dotnet-developer-conference-25'))
AS SOURCE (ShindigPermalink,
           CFPTypeId,
           StartDate,
           EndDate,
           TravelExpensesCovered,
           AccomodationsProvided,
           EventFeesCovered,
           CFPURL)
ON TARGET.ShindigPermalink = SOURCE.ShindigPermalink
WHEN MATCHED THEN UPDATE SET TARGET.CFPTypeId             = SOURCE.CFPTypeId,
                             TARGET.StartDate             = SOURCE.StartDate,
                             TARGET.EndDate               = SOURCE.EndDate,
                             TARGET.CFPURL                = SOURCE.CFPURL,
                             TARGET.TravelExpensesCovered = SOURCE.TravelExpensesCovered,
                             TARGET.AccomodationsProvided = SOURCE.AccomodationsProvided,
                             TARGET.EventFeesCovered      = SOURCE.EventFeesCovered
WHEN NOT MATCHED THEN INSERT (ShindigPermalink,
                              CFPTypeId,
                              StartDate,
                              EndDate,
                              CFPURL,
                              TravelExpensesCovered,
                              AccomodationsProvided)
                      VALUES (SOURCE.ShindigPermalink,
                              SOURCE.CFPTypeId,
                              SOURCE.StartDate,
                              SOURCE.EndDate,
                              SOURCE.CFPURL,
                              SOURCE.TravelExpensesCovered,
                              SOURCE.AccomodationsProvided)
WHEN NOT MATCHED BY SOURCE THEN DELETE;
GO