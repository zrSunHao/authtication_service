/****** Object:  View [dbo].[SysCtmView]    Script Date: 2022/6/9 8:01:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysCtmView] AS SELECT C.Id,FR.FileName AS Avatar,C.Name,C.Nickname,(CASE WHEN CTT.Limited = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END) AS Limited, SR.SysId,S.Name AS SysName,SR.Id AS RoleId,SR.Rank AS RoleRank,SR.Name AS RoleName, CL.CreatedAt AS LastLoginAt, C.Remark,C.CreatedAt
FROM CustomerRoleRelation AS CRR
JOIN Customer AS C ON C.Id = CRR.CustomerId
JOIN SysRole AS SR ON SR.Id = CRR.RoleId
JOIN Sys AS S ON S.Id = SR.SysId
LEFT JOIN (SELECT L1.* FROM UserLastLoginRecord AS L1 JOIN (SELECT CustomerId,MAX(Id) AS Id FROM UserLastLoginRecord GROUP BY CustomerId) AS L2 ON L2.Id = L1.Id) AS CL ON CL.CustomerId = CRR.CustomerId
LEFT JOIN (SELECT F1.* FROM FileResource AS F1 JOIN (SELECT OwnId, MAX(Id) AS Id FROM FileResource WHERE Category = 'ctm' GROUP BY OwnId) AS F2 ON F2.Id = F1.Id) AS FR ON FR.OwnId = C.Id
LEFT JOIN (SELECT CTT2.TargetId, CTT2.SysId,(CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END) AS Limited FROM (SELECT * FROM [Constraint] AS CTT1 WHERE CTT1.Cancelled = 0 AND CTT1.Category = 2 AND (CTT1.Method = 1 OR CTT1.ExpiredAt > GETDATE())) AS CTT2 GROUP BY TargetId, SysId) AS CTT ON CTT.TargetId = CRR.CustomerId AND CTT.SysId = SR.SysId
WHERE C.Deleted = 0 AND SR.Deleted = 0
GO