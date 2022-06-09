/****** Object:  View [dbo].[CttView]    Script Date: 2022/6/9 7:59:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CttView] AS SELECT CTT.Id,CTM.Id AS CtmId,CTM.Name AS CtmName,(CASE WHEN CTT.Category = 4 THEN SR.SysId ELSE S.Id END) AS SysId,(CASE WHEN CTT.Category = 4 THEN SR.RSName ELSE S.Name END) AS SysName,
PF.Id AS FunctId,PF.Name AS FunctName,CTT.Category,CTT.Method,CTT.Origin,CTT.Remark ,CTT.ExpiredAt,CTT.CreatedAt
FROM [Constraint] AS CTT
LEFT JOIN (SELECT * FROM Customer WHERE Deleted = 0) AS CTM ON CTM.Id = CTT.TargetId
LEFT JOIN (SELECT R.*,RS.Name AS RSName FROM SysRole AS R JOIN Sys AS RS ON R.SysId = RS.Id WHERE RS.Deleted = 0) AS SR ON SR.Id = CTT.TargetId
LEFT JOIN (SELECT * FROM Sys WHERE Deleted = 0) AS S ON S.Id = CTT.SysId
LEFT JOIN (SELECT * FROM ProgramFunction WHERE Deleted = 0) AS PF ON PF.Id = CTT.TargetId
WHERE CTT.Cancelled = 0 AND (CTT.Method = 1 OR CTT.ExpiredAt > GETDATE())
GO