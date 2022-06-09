/****** Object:  View [dbo].[CtmView]    Script Date: 2022/6/9 7:59:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CtmView] AS SELECT C.Id,FR.FileName AS Avatar,C.Name,C.Nickname, (CASE WHEN CTT.Limited = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END) AS Limited,CL.CreatedAt AS LastLoginAt, C.Remark,C.CreatedAt
FROM Customer AS C 
LEFT JOIN (SELECT L1.* FROM UserLastLoginRecord AS L1 JOIN (SELECT CustomerId,MAX(Id) AS Id FROM UserLastLoginRecord GROUP BY CustomerId) AS L2 ON L2.Id = L1.Id) AS CL ON CL.CustomerId = C.Id
LEFT JOIN (SELECT F1.* FROM FileResource AS F1 JOIN (SELECT OwnId, MAX(Id) AS Id FROM FileResource WHERE Category = 'ctm' GROUP BY OwnId) AS F2 ON F2.Id = F1.Id) AS FR ON FR.OwnId = C.Id
LEFT JOIN (SELECT CTT2.TargetId, (CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END) AS Limited FROM (SELECT * FROM [Constraint] AS CTT1 WHERE CTT1.Cancelled = 0 AND (CTT1.Method = 1 OR CTT1.ExpiredAt > GETDATE())) AS CTT2 GROUP BY TargetId) AS CTT ON CTT.TargetId = C.Id
WHERE C.Deleted = 0
GO