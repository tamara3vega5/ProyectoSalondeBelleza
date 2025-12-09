-------------------------------------------------------

--  CREAR BASE DE DATOS

-------------------------------------------------------

CREATE DATABASE MatchaSalon;

GO

USE MatchaSalon;

GO
 
 
-------------------------------------------------------

--  TABLA CLIENTES

-------------------------------------------------------

CREATE TABLE Clientes (

    IdCliente INT IDENTITY(1,1) PRIMARY KEY,

    Nombre NVARCHAR(100) NOT NULL,

    Telefono NVARCHAR(20),

    Correo NVARCHAR(100),
 
    -- NUEVO: HASH REAL (NO CONTRASEÑA EN TEXTO PLANO)

    PasswordHash NVARCHAR(255) NOT NULL,
 
    -- NUEVO: ROL PARA ADMINISTRADOR O CLIENTE

    Rol NVARCHAR(50) NOT NULL DEFAULT 'cliente',
 
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()

);

GO
 
 
-------------------------------------------------------

--  TABLA ESTILISTAS

-------------------------------------------------------

CREATE TABLE Estilistas (

    IdEstilista INT IDENTITY(1,1) PRIMARY KEY,

    Nombre NVARCHAR(100) NOT NULL,

    Especialidad NVARCHAR(100),

    Telefono NVARCHAR(20),

    Correo NVARCHAR(100),

    Estado BIT DEFAULT 1

);

GO
 
 
-------------------------------------------------------

--  TABLA SERVICIOS

-------------------------------------------------------

CREATE TABLE Servicios (

    IdServicio INT IDENTITY(1,1) PRIMARY KEY,

    NombreServicio NVARCHAR(100) NOT NULL,

    Precio DECIMAL(10,2) NOT NULL,

    DuracionMin INT NOT NULL,

    Descripcion NVARCHAR(255)

);

GO
 
 
-------------------------------------------------------

--  TABLA CITAS

-------------------------------------------------------

CREATE TABLE Citas (

    IdCita INT IDENTITY(1,1) PRIMARY KEY,

    IdCliente INT NOT NULL,

    IdEstilista INT NOT NULL,

    IdServicio INT NOT NULL,

    Fecha DATETIME NOT NULL,

    Estado NVARCHAR(50) DEFAULT 'Pendiente',
 
    FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente),

    FOREIGN KEY (IdEstilista) REFERENCES Estilistas(IdEstilista),

    FOREIGN KEY (IdServicio) REFERENCES Servicios(IdServicio)

);

GO
 
 
-------------------------------------------------------

--  TABLA PRODUCTOS

-------------------------------------------------------

CREATE TABLE Productos (

    IdProducto INT IDENTITY(1,1) PRIMARY KEY,

    NombreProducto NVARCHAR(100) NOT NULL,

    Precio DECIMAL(10,2) NOT NULL,

    Stock INT NOT NULL,

    Descripcion NVARCHAR(255)

);

GO
 
 
-------------------------------------------------------

--  TABLA VENTAS

-------------------------------------------------------

CREATE TABLE Ventas (

    IdVenta INT IDENTITY(1,1) PRIMARY KEY,

    IdCliente INT NOT NULL,

    Fecha DATETIME NOT NULL DEFAULT GETDATE(),

    Total DECIMAL(10,2) NOT NULL,

    FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente)

);

GO
 
 
-------------------------------------------------------

--  TABLA DETALLE VENTA

-------------------------------------------------------

CREATE TABLE DetalleVenta (

    IdDetalle INT IDENTITY(1,1) PRIMARY KEY,

    IdVenta INT NOT NULL,

    IdProducto INT NOT NULL,

    Cantidad INT NOT NULL,

    Subtotal DECIMAL(10,2) NOT NULL,
 
    FOREIGN KEY (IdVenta) REFERENCES Ventas(IdVenta),

    FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)

);

GO
 
 
-------------------------------------------------------

-- INSERT DE ESTILISTAS

-------------------------------------------------------

INSERT INTO Estilistas (Nombre, Especialidad, Telefono, Correo)

VALUES

('Emilia', 'Cabello', '6000-0001', 'emilia@matchasalon.com'),

('Carlos', 'Cabello', '6000-0002', 'carlos@matchasalon.com'),

('Antonella', 'Uñas', '6000-0003', 'antonella@matchasalon.com'),

('Sofía', 'Faciales', '6000-0004', 'sofia@matchasalon.com'),

('Diego', 'Faciales', '6000-0005', 'diego@matchasalon.com'),

('Ana', 'Maquillaje', '6000-0006', 'ana@matchasalon.com');

GO
 
 
-------------------------------------------------------

-- INSERT DE PRODUCTOS PRINCIPALES

-------------------------------------------------------

INSERT INTO Productos (NombreProducto, Precio, Stock, Descripcion)

VALUES

('Shampoo Herbal', 12.00, 20, 'Repara y fortalece desde el interior'),

('Acondicionador Nutrisoft', 14.00, 18, 'Hidratación profunda y brillo natural'),

('Cera de Cabello', 15.00, 25, 'Cera para estilizar tus peinados favoritos'),

('Gel para Cabello', 13.00, 30, 'Gel para definir rizos todo el día'),

('Aceite de Cabello', 28.00, 10, 'Reduce el frizz sin dejar grasa'),

('Plancha Profesional Titanium Pro', 39.99, 5, 'Plancha profesional de titanio'),

('Secadora', 39.99, 6, 'Secadora profesional de alta velocidad');

GO
 
 
-------------------------------------------------------

-- INSERT ADMINISTRADOR

-------------------------------------------------------

INSERT INTO Clientes (Nombre, Telefono, Correo, PasswordHash, Rol)

VALUES ('Administrador', '0000-0000', 'admin@matcha.com', '1234', 'admin');

GO
 
 
-------------------------------------------------------

-- PROCEDIMIENTO: REGISTRAR CITA

-------------------------------------------------------

CREATE PROCEDURE sp_RegistrarCita

    @IdCliente INT,

    @IdEstilista INT,

    @IdServicio INT,

    @Fecha DATETIME

AS

BEGIN

    IF EXISTS (SELECT 1 FROM Citas WHERE IdEstilista = @IdEstilista AND Fecha = @Fecha)

    BEGIN

        RAISERROR('El estilista ya tiene una cita en esa fecha y hora.', 16, 1);

        RETURN;

    END
 
    INSERT INTO Citas (IdCliente, IdEstilista, IdServicio, Fecha, Estado)

    VALUES (@IdCliente, @IdEstilista, @IdServicio, @Fecha, 'Pendiente');

END;

GO
 
 
-------------------------------------------------------

-- PROCEDIMIENTO: ACTUALIZAR ESTADO DE CITA

-------------------------------------------------------

CREATE PROCEDURE sp_ActualizarEstadoCita

    @IdCita INT,

    @NuevoEstado NVARCHAR(50)

AS

BEGIN

    UPDATE Citas

    SET Estado = @NuevoEstado

    WHERE IdCita = @IdCita;

END;

GO

 