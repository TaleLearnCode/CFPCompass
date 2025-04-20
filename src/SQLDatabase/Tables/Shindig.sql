CREATE TABLE dbo.Shindig
(
  Permalink           VARCHAR(200)   NOT NULL,
  ShindigTypeId       INT            NOT NULL,
  ShindigStatusId     INT            NOT NULL,
  ShindigName         NVARCHAR(200)  NOT NULL,
  CountryCode         CHAR(2)        NOT NULL,
  CountryDivisionCode CHAR(3)            NULL,
  City                NVARCHAR(100)  NOT NULL,
  Venue               NVARCHAR(200)  NOT NULL,
  StartDate           DATE           NOT NULL,
  EndDate             DATE           NOT NULL,
  TimeZoneId          VARCHAR(100)   NOT NULL,
  ShindigDescription  NVARCHAR(2000)     NULL,
  ShindigUrl          VARCHAR(200)       NULL,
  ShindigImageUrl     VARCHAR(200)       NULL,
  IsEnabled           BIT            NOT NULL CONSTRAINT dfShindig_IsEnabled DEFAULT (1),
  CONSTRAINT pkcShindig PRIMARY KEY (Permalink),
  CONSTRAINT fkShindig_ShindigType     FOREIGN KEY (ShindigTypeId)                    REFERENCES dbo.ShindigType (ShindigTypeId),
  CONSTRAINT fkShindig_ShindigStatus   FOREIGN KEY (ShindigStatusId)                  REFERENCES dbo.ShindigStatus (ShindigStatusId),
  CONSTRAINT fkShindig_Country         FOREIGN KEY (CountryCode)                      REFERENCES dbo.Country (CountryCode),
  CONSTRAINT fkShindig_CountryDivision FOREIGN KEY (CountryCode, CountryDivisionCode) REFERENCES dbo.CountryDivision (CountryCode, CountryDivisionCode),
  CONSTRAINT fkShindig_TimeZone        FOREIGN KEY (TimeZoneId)                       REFERENCES dbo.TimeZone (TimeZoneId)
)
