/****** Object:  View [dbo].[SysRoleFunctView]    Script Date: 2022/6/9 8:02:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SysRoleFunctView] AS SELECT SR.Id,SR.Name,SR.Code,S.Id AS SysId,S.Code AS SysCode,P.Id AS PgmId,P.Code AS PgmCode, PF.SectionId AS SectId,PF.Id AS FunctId,PF.Code AS FunctCode,PF.Name AS FunctName,CTT.Limited
FROM SysRoleFuncRelation AS SRFR
JOIN SysRole AS SR ON SR.Id = SRFR.RoleId
JOIN Sys AS S ON S.Id = SR.SysId
JOIN Program AS P ON P.Id = SRFR.ProgramId
JOIN ProgramFunction AS PF ON PF.Id = SRFR.TargetId
LEFT JOIN (SELECT CTT2.TargetId, (CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END) AS Limited FROM (SELECT * FROM [Constraint] AS CTT1 WHERE CTT1.Category = 3 AND CTT1.Cancelled = 0 AND (CTT1.Method = 1 OR CTT1.ExpiredAt > GETDATE())) AS CTT2 GROUP BY TargetId) AS CTT ON CTT.TargetId = PF.Id
WHERE SRFR.IsFunction = 1 AND SR.Deleted = 0 AND S.Deleted = 0 AND PF.Deleted = 0
GO
