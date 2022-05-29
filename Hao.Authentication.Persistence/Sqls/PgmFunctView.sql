USE [Test_AuthenticationPlatform]
GO

/****** Object:  View [dbo].[PgmFunctView]    Script Date: 2022/5/29 17:16:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[PgmFunctView] AS SELECT PF.Id, PF.Name, PF.Code, PF.SectionId AS SectId, PF.ProgramId AS PgmId, CTT.Method AS CttMethod, CTT.ExpiredAt AS LimitedExpiredAt, PF.Remark, PF.CreatedAt
FROM ProgramFunction AS PF
LEFT JOIN (SELECT * FROM [Constraint] WHERE Cancelled = 0 AND (Method = 1 OR ExpiredAt > GETDATE())) AS CTT ON CTT.TargetId = PF.Id
WHERE PF.Deleted = 0
GO

