/****** Object:  View [dbo].[SysView]    Script Date: 2022/6/9 8:03:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysView] AS SELECT S.Id, S.Name,S.Code,FR.FileName AS Logo,S.Intro,S.Remark,S.CreatedAt
FROM Sys AS S
LEFT JOIN (SELECT F1.* FROM FileResource AS F1 JOIN (SELECT OwnId, MAX(Id) AS Id FROM FileResource WHERE Category = 'sys' GROUP BY OwnId) AS F2 ON F2.Id = F1.Id) AS FR ON FR.OwnId = S.Id
WHERE S.Deleted = 0
GO