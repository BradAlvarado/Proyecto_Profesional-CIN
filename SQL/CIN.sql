USE CIN_db;
GO

-- Ver la lista de tablas en la base de datos actual
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';

CREATE TABLE Roles (
    id_rol INT IDENTITY(1,1) PRIMARY KEY,
    nombre_Rol VARCHAR(50)
);
CREATE TABLE Usuarios (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre_u VARCHAR(50),
    clave VARCHAR(50),
    correo_u VARCHAR(50),
    id_rol INT
);


CREATE TABLE Encargados (
    cedula_e VARCHAR(255) PRIMARY KEY,
    responsable_de VARCHAR(50),
    nombre_e VARCHAR(50),
    apellidos_e VARCHAR(60),
    edad_e INT,
    direccion_e VARCHAR(50),
    telefono_e VARCHAR(10),
    correo_e VARCHAR(50),
    lugar_trabajo_e VARCHAR(50)
);

CREATE TABLE PME (   
Cedula VARCHAR(25) PRIMARY KEY,    
poliza_seguro VARCHAR(50),    
nombre_pme VARCHAR(50),    
apellidos_pme VARCHAR(60),    
edad_pme INT,    
fecha_nace_pme DATE,    
provincia_pme VARCHAR(20),    
canton_pme VARCHAR(20),    
distrito_pme VARCHAR(20),    
nacionalidad_pme VARCHAR(20),    
recibe_subvencion BIT,    
fechaIngreso DATE,    
fechaEgreso DATE,    
condición_migratoria_pme VARCHAR(50),    
genero_pme VARCHAR(20),    
nivel_educativo_pme VARCHAR(50),   
responsable__pme VARCHAR(50)
); 
 



Drop table encargados;	
