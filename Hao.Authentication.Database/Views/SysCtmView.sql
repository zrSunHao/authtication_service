USE [Test_AuthenticationPlatform]
GO

/****** Object:  View [dbo].[SysCtmView]    Script Date: 2022/5/26 18:53:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysCtmView] AS SELECT C.Id,FR.FileName AS Avatar,C.Name,C.Nickname,CTT.Limited, SR.Id AS RoleId,SR.Rank AS RoleRank,SR.Name AS RoleName, CL.CreatedAt AS LastLoginAt, C.Remark,C.CreatedAt
FROM CustomerRoleRelation AS CRR
JOIN Customer AS C ON C.Id = CRR.CustomerId
JOIN SysRole AS SR ON SR.Id = CRR.RoleId
LEFT JOIN (SELECT L1.* FROM CustomerLog AS L1 JOIN (SELECT CustomerId,MAX(Id) AS Id FROM CustomerLog  WHERE Operate = 'login' GROUP BY CustomerId) AS L2 ON L2.Id = L1.Id) AS CL ON CL.CustomerId = C.Id
LEFT JOIN (SELECT F1.* FROM FileResource AS F1 JOIN (SELECT OwnId, MAX(Id) AS Id FROM FileResource WHERE Category = 'ctm' GROUP BY OwnId) AS F2 ON F2.Id = F1.Id) AS FR ON FR.OwnId = C.Id
LEFT JOIN (SELECT CTT2.TargetId, (CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END) AS Limited FROM (SELECT * FROM [Constraint] AS CTT1 WHERE CTT1.Cancelled = 0 AND (CTT1.Method = 1 OR CTT1.ExpiredAt > GETDATE())) AS CTT2 GROUP BY TargetId) AS CTT ON CTT.TargetId = C.Id
WHERE C.Deleted = 0 AND SR.Deleted = 0
GO

