CREATE DATABASE CIN;
GO

USE CIN_prueba;

GO

CREATE TABLE Usuarios (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
	foto_u VARCHAR(300),
    nombre_u VARCHAR(50) not null,
	correo_u VARCHAR(50) not null,
    clave VARCHAR(50) not null,
	token VARCHAR(100),
	estado_u BIT, -- ACTIVO - INACTIVO 
	acceso_u BIT,
	id_rol INT
);


CREATE TABLE Roles (
    id_rol INT IDENTITY(1,1) PRIMARY KEY,
    nombre_rol VARCHAR(50) not null
);

CREATE TABLE Modulos (
    id_modulo INT IDENTITY (1,1) PRIMARY KEY,
    nombre_modulo VARCHAR(50)
);


CREATE TABLE Permisos (
    id_rol INT,
    id_modulo INT,
    permitido BIT
);

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
	ADD FOREIGN KEY (id_rol) REFERENCES Roles(id_rol)
-----------------------------------------

ALTER TABLE PERMISOS
	ADD FOREIGN KEY (id_rol) REFERENCES Roles(id_rol),
	FOREIGN KEY (id_modulo) REFERENCES Modulos(id_modulo);
-----------------------------------------

ALTER TABLE Personal
	ADD FOREIGN KEY (id_rol) REFERENCES Roles(id_rol);
-----------------------------------------

ALTER TABLE PME
	ADD id_encargado int FOREIGN KEY (id_encargado) REFERENCES Encargados(id_encargado);
-----------------------------------------
ALTER TABLE Encargados
	ADD id_pme int FOREIGN KEY (id_pme) REFERENCES PME(id_pme);

	
-----------------------------------------

-----------------------------------------
----------------------------------================---------------------------------------
------ TERMINA ALTERS -------------

-------------------------------------

------ CREATE STORED PROCEDURES -------------

CREATE PROCEDURE SP_Registrarse
	@Foto VARBINARY(MAX),
    @NombreU VARCHAR(50),
	@Correo VARCHAR(50),
    @Clave VARCHAR(50),
	@Token VARCHAR(100),
	@Estado BIT, -- ACTIVO - INACTIVO 
	@Acceso BIT,
	@IdRol INT
AS BEGIN
	INSERT INTO Usuarios VALUES(@Foto, @NombreU, @Correo, @Clave, @Token,@Estado,@Acceso, @IdRol)
END

CREATE PROCEDURE SP_Validar
	@Token VARCHAR(100)
AS BEGIN
	DECLARE @Correo VARCHAR(50)
	SET @Correo =(SELECT correo_u FROM Usuarios where token = @Token)
	UPDATE Usuarios SET estado_u=1 WHERE token = @Token
	UPDATE Usuarios SET token = null WHERE correo_u = @Correo
END