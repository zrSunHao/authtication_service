USE [Test_AuthenticationPlatform]
GO

/****** Object:  View [dbo].[SysRoleView]    Script Date: 2022/5/30 17:19:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysRoleView] AS SELECT SR.Id,SR.Rank,SR.Name,SR.Code,S.Id AS SysId,S.Name AS SysName,S.Code AS SysCode,CTT.Method AS CttMethod,CTT.ExpiredAt AS LimitedExpiredAt,SR.Remark,SR.CreatedAt
FROM SysRole AS SR
JOIN Sys AS S ON S.Id = SR.SysId
LEFT JOIN (SELECT * FROM [Constraint] WHERE Cancelled = 0 AND (Method = 1 OR ExpiredAt > GETDATE())) AS CTT ON CTT.TargetId = SR.Id
WHERE SR.Deleted = 0 AND S.Deleted = 0
GO

