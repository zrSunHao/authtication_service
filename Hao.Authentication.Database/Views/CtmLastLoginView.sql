USE [Test_AuthenticationPlatform]
GO

/****** Object:  View [dbo].[CtmLastLoginView]    Script Date: 2022/5/26 18:52:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CtmLastLoginView] AS SELECT C.Id AS Id,C.Name AS Name, FR.FileName AS Avatar, S.Id AS SysId, S.Name AS SysName
FROM Customer AS C 
LEFT JOIN (SELECT L1.* FROM CustomerLog AS L1 JOIN (SELECT CustomerId,MAX(Id) AS Id FROM CustomerLog  WHERE Operate = 'login' GROUP BY CustomerId) AS L2 ON L2.Id = L1.Id) AS CL ON CL.CustomerId = C.Id
LEFT JOIN Sys AS S ON S.Id = CL.SystemId
LEFT JOIN (SELECT F1.* FROM FileResource AS F1 JOIN (SELECT OwnId, MAX(Id) AS Id FROM FileResource WHERE Category = 'ctm' GROUP BY OwnId) AS F2 ON F2.Id = F1.Id) AS FR ON FR.OwnId = C.Id
WHERE C.Deleted = 0 AND S.Deleted = 0
GO

