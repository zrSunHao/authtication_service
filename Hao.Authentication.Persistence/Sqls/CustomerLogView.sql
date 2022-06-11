/****** Object:  View [dbo].[CustomerLogView]    Script Date: 2022/6/11 10:13:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CustomerLogView] AS SELECT
CL.Id,
CL.CustomerId AS CtmId,
C.Name AS CtmName,
C.Nickname AS CtmNickname,
CL.Operate,
CL.SystemId AS SysId,
S.Name AS SysName,
S.Code AS SysCode,
CL.ProgramId AS PgmId,
P.Name AS PgmName,
P.Code AS PgmCode,
CL.RoleId AS RoleId,
R.Name AS RoleName,
R.Code AS RoleCode,
R.Rank AS RoleRank,
CL.RemoteAddress,
CL.Remark,
CL.CreatedAt
FROM CustomerLog AS CL
LEFT JOIN Sys AS S ON CL.SystemId = S.Id
LEFT JOIN SysRole AS R ON CL.RoleId = R.Id
LEFT JOIN Program AS P ON CL.ProgramId = P.Id
LEFT JOIN Customer AS C ON CL.CustomerId = C.Id
GO


