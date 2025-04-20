CREATE TABLE dbo.CFP
(
  ShindigPermalink      VARCHAR(200)   NOT NULL,
  CFPTypeId             INT            NOT NULL,
  StartDate             DATE           NOT NULL,
  EndDate               DATE           NOT NULL,
  CFPURL                VARCHAR(200)   NOT NULL,
  TravelExpensesCovered BIT            NOT NULL CONSTRAINT dfCFP_TravelExpensesCovered DEFAULT (0),
  AccomodationsProvided BIT            NOT NULL CONSTRAINT dfCFP_AccomodationsProvided DEFAULT (0),
  EventFeesCovered      BIT            NOT NULL CONSTRAINT dfCFP_EventFeesCovered DEFAULT (1),
  AdditionalBenefits    NVARCHAR(2000)     NULL,
  CONSTRAINT pkcCFP PRIMARY KEY (ShindigPermalink),
  CONSTRAINT fkCFP_Shindig FOREIGN KEY (ShindigPermalink) REFERENCES dbo.Shindig(Permalink),
  CONSTRAINT fkCFP_CFPType FOREIGN KEY (CFPTypeId) REFERENCES dbo.CFPType(CFPTypeId)
)