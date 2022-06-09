/****** Object:  View [dbo].[CtmRoleView]    Script Date: 2022/6/9 7:52:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CtmRoleView] AS SELECT CR.CustomerId AS Id,S.Id AS SysId,S.Name AS SysName,FR.FileName AS SysLogo,SR.Id AS RoleId,SR.Name AS RoleName, SR.Rank AS Rank,CR.Remark AS Remark,CR.CreatedAt AS CreatedAt 
FROM CustomerRoleRelation AS CR
JOIN SysRole AS SR ON SR.Id = CR.RoleId
JOIN Sys AS S ON S.Id = SR.SysId
LEFT JOIN (SELECT F1.* FROM FileResource AS F1 JOIN (SELECT OwnId, MAX(Id) AS Id FROM FileResource WHERE Category = 'sys' GROUP BY OwnId) AS F2 ON F2.Id = F1.Id) AS FR ON FR.OwnId = S.Id
WHERE S.Deleted = 0 AND SR.Deleted = 0
GO
