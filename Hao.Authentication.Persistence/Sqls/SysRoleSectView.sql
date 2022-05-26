USE [Test_AuthenticationPlatform]
GO

/****** Object:  View [dbo].[SysRoleSectView]    Script Date: 2022/5/26 18:53:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysRoleSectView] AS SELECT SR.Id,SR.Name,SR.Code,S.Id AS SysId,S.Code AS SysCode,PS.ProgramId AS PgmId,PS.Id AS SectId,PS.Code AS SectCode,PS.Name AS SectName,PS.Category AS Category
FROM SysRoleFuncRelation AS SRFR
JOIN SysRole AS SR ON SR.Id = SRFR.RoleId
JOIN Sys AS S ON S.Id = SR.SysId
JOIN ProgramSection AS PS ON PS.Id = SRFR.TargetId
WHERE SRFR.IsFunction = 0 AND SR.Deleted = 0 AND S.Deleted = 0 AND PS.Deleted = 0
GO

