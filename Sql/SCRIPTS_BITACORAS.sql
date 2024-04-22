
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


SELECT * FROM BITACORA_MOVIMIENTOS;


SELECT * FROM PME;

INSERT INTO PME (cedula_pme,nombre_pme, apellidos_pme, fecha_nacimiento_pme,edad_pme, provincia_pme, genero_pme, nacionalidad_pme, fecha_ingreso_pme)
values('231253442','Simon', 'Mejia', '2010-03-25 15:28:28.790', 5, 'Heredia', 'M','Paisa', '2021-03-25 15:28:28.790');

UPDATE PME 
SET provincia_pme = 'Limon'
WHERE cedula_pme = '231253442';

DELETE FROM PME WHERE cedula_pme = '231253442';

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


---- Carlos


---- BRAD
UPDATE Usuarios 
SET estado_u = 1
WHERE id_usuario = 3;

---- ISAAC
UPDATE Usuarios 
SET estado_u = 1
WHERE id_usuario = 5;

UPDATE Usuarios 
SET estado_u = 1
WHERE id_usuario = 6;

UPDATE Usuarios 
SET estado_u = 1
WHERE id_usuario = 6;

select * from Bitacora_ingreso_salida;
SELECT * FROM Usuarios;

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
values('Isaac','isaac@gmail.com', 'i1234', 1,1,1);



DELETE Usuarios where id_usuario = 15
