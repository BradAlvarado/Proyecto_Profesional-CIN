Create DataBase SistemaCIN_db

USE SistemaCIN_db;

GO

CREATE TABLE Usuarios (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
	foto_u VARCHAR(max),
    nombre_u VARCHAR(50) not null,
	correo_u VARCHAR(50) not null,
    clave VARCHAR(100) not null,
	estado_u BIT, -- ACTIVO - INACTIVO 
	acceso_u BIT,
	id_rol INT -- FK 1 -- FK 2
);


------ LO QUE NOS AYUDA CON ROLES Y PERMISOS 
CREATE TABLE Rol (
    id_rol INT IDENTITY(1,1) PRIMARY KEY, -- FK 1
    nombre_rol VARCHAR(50) not null
);
CREATE TABLE Rol_operacion (
    id_rol_op INT IDENTITY(1,1) PRIMARY KEY,
    id_rol INT, -- FK 2
    id_op INT -- FK 3
);

CREATE TABLE Operaciones(
    id_op INT IDENTITY(1,1) PRIMARY KEY, -- FK 3
    nombre_op VARCHAR(20) not null,
    id_modulo INT -- FK 4
);


CREATE TABLE Modulos (
    id_modulo INT IDENTITY (1,1) PRIMARY KEY, -- FK 4
    nombre_modulo VARCHAR(50)
);
------ LO QUE NOS AYUDA CON ROLES Y PERMISOS 

CREATE TABLE Bitacora_ingreso_salida(
	id_bitacora INT IDENTITY(1,1) PRIMARY KEY,
	usuario_b VARCHAR(50),
	fecha_ingreso DATETIME,
	fecha_salida DATETIME,
	estado_actual int
);


CREATE TABLE Bitacora_movimientos(
	id_bitacora INT IDENTITY(1,1) PRIMARY KEY,
	usuario_b VARCHAR(50),
	fecha_movimiento DATETIME,
	tipo_movimiento VARCHAR(20),
	detalle VARCHAR (50)
);


CREATE TABLE PME (
	id_pme INT IDENTITY(1,1) PRIMARY KEY,
    id_encargado INT,
    cedula_pme VARCHAR(25) not null,
    poliza_seguro VARCHAR(50),
    nombre_pme VARCHAR(50) not null,
    apellidos_pme VARCHAR(60) not null,
	fecha_nacimiento_pme DATE not null,
    edad_pme INT not null,
	genero_pme VARCHAR(20) not null,
    provincia_pme VARCHAR(20) not null,
    canton_pme VARCHAR(20),
    distrito_pme VARCHAR(20),
    nacionalidad_pme VARCHAR(20) not null,
    subvencion_pme BIT,
    fecha_ingreso_pme DATE not null,
    fecha_egreso_pme DATETIME,
    condición_migratoria_pme VARCHAR(50),
    nivel_educativo_pme VARCHAR(50)
);


CREATE TABLE Encargados (
	id_encargado INT IDENTITY(1,1) PRIMARY KEY,
    id_pme INT,
    cedula_e VARCHAR(25) not null,
    nombre_e VARCHAR(50) not null,
    apellidos_e VARCHAR(60) not null,
    fecha_nace_e DATE,
	edad int not null,
	correo_e VARCHAR(50)not null,
    direccion_e VARCHAR(60) not null,
    telefono_e VARCHAR(10) not null,
    lugar_trabajo_e VARCHAR(25)
);


CREATE TABLE Personal (
	id_personal INT IDENTITY(1,1) PRIMARY KEY,
	cedula_p VARCHAR(25) not null,
    nombre_p VARCHAR(50) not null,
    apellidos_p VARCHAR(60) not null,
	correo_p VARCHAR(50) not null, 
    telefono_p VARCHAR(12),
	fecha_nace_p DATE,
    edad_p INT not null,
	genero_p VARCHAR(20) not null,
    provincia_p VARCHAR(20) not null,
    canton_p VARCHAR(20),
    distrito_p VARCHAR(20), 
	id_rol INT  
);


-----------------------------------------
--- ALTERS LLAVES FORANEAS --------------

ALTER TABLE Usuarios
	ADD FOREIGN KEY (id_rol) REFERENCES Rol(id_rol)
-----------------------------------------
ALTER TABLE Rol_operacion
	ADD FOREIGN KEY (id_rol) REFERENCES Rol(id_rol),
    FOREIGN KEY (id_op) REFERENCES Operaciones(id_op)

ALTER TABLE Operaciones
	ADD FOREIGN KEY (id_modulo) REFERENCES Modulos(id_modulo)

-----------------------------------------

ALTER TABLE Personal
	ADD FOREIGN KEY (id_rol) REFERENCES Rol(id_rol);
-----------------------------------------

ALTER TABLE PME
	ADD FOREIGN KEY (id_encargado) REFERENCES Encargados(id_encargado);
-----------------------------------------
ALTER TABLE Encargados
	ADD FOREIGN KEY (id_pme) REFERENCES PME(id_pme);



----------------------bitacoras------------------------------

CREATE TRIGGER TR_AuditarCambiosPME
ON PME
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @Usuario VARCHAR(50)
    DECLARE @Movimiento VARCHAR(10)
    DECLARE @PMEAfectado VARCHAR(100)
    DECLARE @FechaActual DATETIME
	DECLARE @Detalle VARCHAR(100)

    SET @Usuario = SYSTEM_USER
    SET @FechaActual = GETDATE()

    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        IF EXISTS (SELECT * FROM deleted)
		BEGIN
            SET @Movimiento = 'UPDATE'; -- Actualización
			SET @PMEAfectado = (SELECT nombre_pme FROM inserted)
		END
        ELSE
		BEGIN
            SET @Movimiento = 'INSERT'; -- Inserción
			SET @PMEAfectado = (SELECT nombre_pme FROM inserted)
		END
	END
    ELSE IF EXISTS(SELECT * FROM deleted)
		BEGIN
        SET @Movimiento = 'DELETE'; -- Eliminación
		SET @PMEAfectado = (SELECT nombre_pme FROM deleted)
	END
	
	SET @Detalle = (@Usuario + ' ' + @Movimiento + ' ' + @PMEAfectado);

    INSERT INTO Bitacora_movimientos(usuario_b, fecha_movimiento, tipo_movimiento, detalle)
    VALUES (@Usuario, @FechaActual, @Movimiento, @Detalle)
END

---------------------------------------------------
---------------------------------------------------
---------------------------------------------------
---------------------------------------------------
---------------------------------------------------


--SELECT * FROM BITACORA_MOVIMIENTOS;


--SELECT * FROM PME;

--INSERT INTO PME (cedula_pme,nombre_pme, apellidos_pme, fecha_nacimiento_pme,edad_pme, provincia_pme, genero_pme, nacionalidad_pme, fecha_ingreso_pme)
--values('231253442','Simon', 'Mejia', '2010-03-25 15:28:28.790', 5, 'Heredia', 'M','Paisa', '2021-03-25 15:28:28.790');

--UPDATE PME 
--SET provincia_pme = 'Limon'
--WHERE cedula_pme = '231253442';

--DELETE FROM PME WHERE cedula_pme = '231253442';

---------------------------------------------------
---------------------------------------------------
---------------------------------------------------

---------------------------------------------------
------------------------------------------------
drop trigger TR_Insertar_Bitacora_Ingreso

CREATE TRIGGER TR_Insertar_Bitacora_Ingreso
ON Usuarios
AFTER INSERT, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Variables para almacenar información relevante
    DECLARE @Usuario VARCHAR(50);
    DECLARE @FechaActual DATETIME;

    -- Obtener la fecha y hora actual en UTC
    SET @FechaActual = GETDATE();

    -- Convertir a la hora local de Costa Rica (UTC-6)
    SET @FechaActual = DATEADD(HOUR, -6, @FechaActual);

    -- Insertar un registro en Bitacora_ingreso_salida para el nuevo usuario
    IF EXISTS (SELECT * FROM inserted WHERE estado_u = 1)
    BEGIN
        INSERT INTO Bitacora_ingreso_salida (usuario_b, fecha_ingreso, estado_actual)
        SELECT correo_u, @FechaActual, estado_u
        FROM inserted
        WHERE estado_u = 1; -- Insertar solo si el nuevo usuario tiene estado_u igual a 1
    END;

    -- Eliminar el usuario de Bitacora_ingreso_salida si se ha eliminado de Usuarios
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        DELETE FROM Bitacora_ingreso_salida
        WHERE usuario_b IN (SELECT correo_u FROM deleted);
    END;
END;


----------------------------------------------------

Create TRIGGER TR_Bitacora_Ingreso_Salida
ON Usuarios
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Variables para almacenar información relevante
    DECLARE @Usuario VARCHAR(50);
    DECLARE @FechaActual DATETIME;
    DECLARE @NuevoEstado BIT;
    DECLARE @IdBitacora INT; -- Declarar la variable para almacenar el id_bitacora

    -- Obtener la fecha y hora actual en UTC
    SET @FechaActual = GETDATE();

    -- Convertir a la hora local de Costa Rica (UTC-6)
    SET @FechaActual = DATEADD(HOUR, -6, @FechaActual);

    -- Obtener información de las filas afectadas por la actualización
    SELECT @Usuario = i.correo_u,
           @NuevoEstado = i.estado_u
    FROM inserted i
    INNER JOIN deleted d ON i.id_usuario = d.id_usuario;

    -- Obtener el id_bitacora correspondiente al usuario en Bitacora_ingreso_salida
    SELECT @IdBitacora = id_bitacora
    FROM Bitacora_ingreso_salida
    WHERE usuario_b = @Usuario;

    -- Verificar si el estado cambió de 1 a 0 (de activo a inactivo)
    IF @NuevoEstado = 0
    BEGIN
        -- Actualizar la fecha de salida en la tabla Bitacora_ingreso_salida
        UPDATE Bitacora_ingreso_salida
        SET fecha_salida = @FechaActual,
            estado_actual = 0 -- Actualizar el estado actual en la tabla
        WHERE usuario_b = @Usuario AND id_bitacora = @IdBitacora;
    END
    ELSE
    BEGIN
        -- Actualizar la fecha de ingreso en la tabla Bitacora_ingreso_salida
        UPDATE Bitacora_ingreso_salida
        SET fecha_ingreso = @FechaActual,
            estado_actual = 1 -- Actualizar el estado actual en la tabla
        WHERE usuario_b = @Usuario AND id_bitacora = @IdBitacora;

        -- Verificar si el usuario ya tiene un registro en la tabla Bitacora_ingreso_salida
        IF @IdBitacora IS NULL
        BEGIN
            -- Insertar un nuevo registro solo si el usuario no tiene uno existente
            INSERT INTO Bitacora_ingreso_salida (usuario_b, fecha_ingreso, estado_actual)
            VALUES (@Usuario, @FechaActual, @NuevoEstado);
        END;
    END;
END;


--------------------------------------------------


---- Nuevo usuario
INSERT INTO Usuarios (nombre_u,correo_u, clave, estado_u, acceso_u, id_rol)
values('Vilma','vilma@gmail.com', 'v1234', 1,1,1);

SELECT * FROM Usuarios;
select * from Bitacora_ingreso_salida;
---------------------------------------

UPDATE Usuarios 
SET estado_u = 0
WHERE id_usuario = 15;

select * from Bitacora_ingreso_salida;

DELETE Usuarios where id_usuario = 7
select * from Bitacora_ingreso_salida;


delete from Bitacora_ingreso_salida;


INSERT INTO Usuarios (nombre_u,correo_u, clave, estado_u, acceso_u, id_rol)
values('Brad','bradsistema.cin', 'Admin123#!', 0,1,1);

INSERT INTO Rol (nombre_rol) values('Invitado')

Select * from Modulos;
INSERT INTO Modulos(nombre_modulo) values('Mantenimiento Personal');      -- 1
INSERT INTO Modulos(nombre_modulo) values('Mantenimiento PME');           -- 2
INSERT INTO Modulos(nombre_modulo) values('Mantenimiento Encargados');    -- 3
INSERT INTO Modulos(nombre_modulo) values('Bitacoras Ingresos Salidas');  -- 4
INSERT INTO Modulos(nombre_modulo) values('Bitacoras Pme');               -- 5
INSERT INTO Modulos(nombre_modulo) values('Ayuda');                       -- 6
INSERT INTO Modulos(nombre_modulo) values('Acerca de');                   -- 7

update Usuarios 
set id_rol = 1 where id_usuario = 18;


Select * from Usuarios; 
Select * from Rol; 
Select * from Modulos; 
Select * from Operaciones; 
Select * from Rol_operacion;


DBCC CHECKIDENT (Rol, RESEED, 0);
DBCC CHECKIDENT (Operaciones, RESEED, 0);
DBCC CHECKIDENT (Rol_operacion, RESEED, 0);
DBCC CHECKIDENT (Usuarios, RESEED, 0);

Delete from Rol_operacion;

--Delete from Modulos; -- no borrar
--Delete from Usuarios;

Delete from Rol_operacion where id_rol =9;
Delete from Rol where id_rol = 9;

DELETE Operaciones where id_op = 24

select * from Bitacora_ingreso_salida;

UPDATE Usuarios 
SET id_rol = 1
WHERE correo_u = 'brad@sistema.cin';

-- AQUI SE GENERAN TODAS LAS OPERACIONES POSIBLES EN CADA MODULO
INSERT INTO Operaciones(nombre_op, id_modulo) values('Ver', 7);
INSERT INTO Operaciones(nombre_op, id_modulo) values('Crear',3);
INSERT INTO Operaciones(nombre_op, id_modulo) values('Editar',3);
INSERT INTO Operaciones(nombre_op, id_modulo) values('Reportar',3);
INSERT INTO Operaciones(nombre_op, id_modulo) values('Eliminar',3);
Select * from Operaciones;

Select * from Modulos;
Select * from Rol_operacion;
Select * from Operaciones;
                                         -- Rol 4- SubAdmin 
INSERT INTO Rol_operacion(id_rol, id_op) values(1,1)  -- Ver Personal
INSERT INTO Rol_operacion(id_rol, id_op) values(1,2); -- Crear Personal
INSERT INTO Rol_operacion(id_rol, id_op) values(1,3); -- Editar Personal
INSERT INTO Rol_operacion(id_rol, id_op) values(1,4); -- Reportar Personal
INSERT INTO Rol_operacion(id_rol, id_op) values(1,5); -- Eliminar Personal
INSERT INTO Rol_operacion(id_rol, id_op) values(1,6); -- Ver Pme
INSERT INTO Rol_operacion(id_rol, id_op) values(1,7); -- Crear Pme
INSERT INTO Rol_operacion(id_rol, id_op) values(1,8); -- Editar Pme
INSERT INTO Rol_operacion(id_rol, id_op) values(1,9); -- Reportar Pme
INSERT INTO Rol_operacion(id_rol, id_op) values(1,10); -- Eliminar Pme
INSERT INTO Rol_operacion(id_rol, id_op) values(1,11); -- Ver Encargados
INSERT INTO Rol_operacion(id_rol, id_op) values(1,12); -- Crear Encargados
INSERT INTO Rol_operacion(id_rol, id_op) values(1,13); -- Editar Encargados
INSERT INTO Rol_operacion(id_rol, id_op) values(1,14); -- Reportar Encargados
INSERT INTO Rol_operacion(id_rol, id_op) values(1,15); -- Eliminar Encargados
INSERT INTO Rol_operacion(id_rol, id_op) values(1,16); -- Ver Bitacoras Ingreso Salidas
INSERT INTO Rol_operacion(id_rol, id_op) values(1,17); -- Ver Bitacoras PME
INSERT INTO Rol_operacion(id_rol, id_op) values(1,18); -- Ver Ayuda
INSERT INTO Rol_operacion(id_rol, id_op) values(1,19); -- Ver Acerca de


