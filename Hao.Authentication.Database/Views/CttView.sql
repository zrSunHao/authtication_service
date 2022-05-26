USE [Test_AuthenticationPlatform]
GO

/****** Object:  View [dbo].[CttView]    Script Date: 2022/5/26 18:53:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CttView] AS SELECT CTT.Id,CTM.Id AS CtmId,CTM.Name AS CtmName,(CASE WHEN CTT.Category = 4 THEN SR.SysId ELSE S.Id END) AS SysId,(CASE WHEN CTT.Category = 4 THEN SR.RSName ELSE S.Name END) AS SysName,
PF.Id AS FunctId,PF.Name AS FunctName,CTT.Category,CTT.Method,CTT.Origin,CTT.Remark ,CTT.ExpiredAt,CTT.CreatedAt
FROM [Constraint] AS CTT
LEFT JOIN Customer AS CTM ON CTM.Id = CTT.TargetId
LEFT JOIN (SELECT R.*,RS.Name AS RSName FROM SysRole AS R JOIN Sys AS RS ON R.SysId = RS.Id WHERE RS.Deleted = 0) AS SR ON SR.Id = CTT.TargetId
LEFT JOIN Sys AS S ON S.Id = CTT.TargetId
LEFT JOIN ProgramFunction AS PF ON PF.Id = CTT.TargetId
WHERE CTT.Cancelled = 0 AND (CTT.Method = 1 OR CTT.ExpiredAt > GETDATE()) AND CTM.Deleted = 0 AND SR.Deleted = 0 AND S.Deleted = 0 AND PF.Deleted = 0
GO

