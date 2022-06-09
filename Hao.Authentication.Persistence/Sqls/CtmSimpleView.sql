/****** Object:  View [dbo].[CtmSimpleView]    Script Date: 2022/6/9 7:57:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CtmSimpleView] AS SELECT C.Id,FR.FileName AS Avatar,C.Name,C.Nickname, C.Remark, CI.Intro, C.CreatedAt
FROM Customer AS C 
JOIN CustomerInformation AS CI ON C.Id = CI.CustomerId
LEFT JOIN (SELECT F1.* FROM FileResource AS F1 JOIN (SELECT OwnId, MAX(Id) AS Id FROM FileResource WHERE Category = 'ctm' GROUP BY OwnId) AS F2 ON F2.Id = F1.Id) AS FR ON FR.OwnId = C.Id
WHERE C.Deleted = 0
GO
