USE CIN;
DELETE FROM Roles where id_rol != 1;
DBCC CHECKIDENT ('Roles', RESEED, 1);