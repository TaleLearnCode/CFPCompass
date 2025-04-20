CREATE TABLE dbo.TimeZone
(
  TimeZoneId                  VARCHAR(100) NOT NULL,
  StandardAbbreviation        CHAR(7)      NOT NULL,
  StandardOffset              CHAR(6)      NOT NULL,
  DaylightSavingsAbbreviation CHAR(7)          NULL,
  DaylightOffset              CHAR(6)          NULL,
  CONSTRAINT pkcTimeZone PRIMARY KEY (TimeZoneId)
)
GO

EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'TimeZone',                                              @value=N'Represents the list of time zones as defined by the IANA.',                            @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'TimeZone', @level2name=N'TimeZoneId',                   @value=N'The identifier of the time zone as defined by the IANA.',                              @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'TimeZone', @level2name=N'StandardAbbreviation',         @value=N'The standard abbreviation for the time zone.',                                         @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'TimeZone', @level2name=N'StandardOffset',               @value=N'The standard offset for the time zone.',                                               @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'TimeZone', @level2name=N'DaylightSavingsAbbreviation',  @value=N'The daylight savings abbreviation for the time zone.',                                 @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'TimeZone', @level2name=N'DaylightOffset',               @value=N'The daylight offset for the time zone.',                                               @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'COLUMN';
GO
EXEC sp_addextendedproperty @level0name=N'dbo', @level1name=N'TimeZone', @level2name=N'pkcTimeZone',                  @value=N'Defines the primary key for the TimeZone table using the TimeZoneId column.',          @name=N'MS_Description', @level0type=N'SCHEMA', @level1type=N'TABLE', @level2type=N'CONSTRAINT';
GO