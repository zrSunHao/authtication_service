/****** Object:  View [dbo].[SysRoleSectView]    Script Date: 2022/6/9 8:02:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysRoleSectView] AS SELECT SR.Id,SR.Name,SR.Code,S.Id AS SysId,S.Code AS SysCode,P.Id AS PgmId, P.Code AS PgmCode, PS.Id AS SectId,PS.Code AS SectCode,PS.Name AS SectName,PS.Category AS Category
FROM SysRoleFuncRelation AS SRFR
JOIN SysRole AS SR ON SR.Id = SRFR.RoleId
JOIN Sys AS S ON S.Id = SR.SysId
JOIN Program AS P ON P.Id = SRFR.ProgramId
JOIN ProgramSection AS PS ON PS.Id = SRFR.TargetId
WHERE SRFR.IsFunction = 0 AND SR.Deleted = 0 AND S.Deleted = 0 AND PS.Deleted = 0
GO