USE [Test_AuthenticationPlatform]
GO

/****** Object:  View [dbo].[SysRoleView]    Script Date: 2022/5/26 18:54:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysRoleView] AS SELECT SR.Id,SR.Rank,SR.Name,S.Id AS SysId,S.Name AS SysName,S.Code AS SysCode,CTT.Limited,SR.Remark,SR.CreatedAt
FROM SysRole AS SR
JOIN Sys AS S ON S.Id = SR.SysId
LEFT JOIN (SELECT CTT2.TargetId, (CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END) AS Limited FROM (SELECT * FROM [Constraint] AS CTT1 WHERE CTT1.Cancelled = 0 AND (CTT1.Method = 1 OR CTT1.ExpiredAt > GETDATE())) AS CTT2 GROUP BY TargetId) AS CTT ON CTT.TargetId = SR.Id
WHERE SR.Deleted = 0 AND S.Deleted = 0
GO

