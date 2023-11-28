USE [Banco];

INSERT INTO Administrador (Nombre, NumeroTelefono, CorreoE, Contra, TipoAdmin)
VALUES ('chk', '01800454', 'chk@chk.chk', 'chk', 'su')
INSERT INTO Administrador (Nombre, NumeroTelefono, CorreoE, Contra, TipoAdmin)
VALUES ('mz', '701', 'mz@doblep.com', 'laborandopariente', 'ver')

SELECT TOP (1000) [Id]
      ,[Nombre]
      ,[NumeroTelefono]
      ,[CorreoE]
      ,[Contra]
      ,[TipoAdmin]
      ,[FechaRegistro]
  FROM [Banco].[dbo].[Administrador]