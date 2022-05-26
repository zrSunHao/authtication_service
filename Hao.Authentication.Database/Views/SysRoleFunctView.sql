USE [Test_AuthenticationPlatform]
GO

/****** Object:  View [dbo].[SysRoleFunctView]    Script Date: 2022/5/26 18:53:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysRoleFunctView] AS SELECT SR.Id,SR.Name,SR.Code,S.Id AS SysId,S.Code AS SysCode,PF.ProgramId AS PgmId,PF.SectionId AS SectId,PF.Id AS FunctId,PF.Code AS FunctCode,PF.Name AS FunctName
FROM SysRoleFuncRelation AS SRFR
JOIN SysRole AS SR ON SR.Id = SRFR.RoleId
JOIN Sys AS S ON S.Id = SR.SysId
JOIN ProgramFunction AS PF ON PF.Id = SRFR.TargetId
WHERE SRFR.IsFunction = 1 AND SR.Deleted = 0 AND S.Deleted = 0 AND PF.Deleted = 0
GO

