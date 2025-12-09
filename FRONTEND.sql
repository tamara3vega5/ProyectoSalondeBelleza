CREATE DATABASE MatchaSalon;
USE MatchaSalon;
GO
CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20),
    Correo NVARCHAR(100),
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO


CREATE TABLE Estilistas (
    IdEstilista INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Especialidad NVARCHAR(100),
    Telefono NVARCHAR(20),
    Correo NVARCHAR(100),
    Estado BIT DEFAULT 1  -- 1=Activo, 0=Inactivo
);
GO


CREATE TABLE Servicios (
    IdServicio INT IDENTITY(1,1) PRIMARY KEY,
    NombreServicio NVARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    DuracionMin INT NOT NULL,
    Descripcion NVARCHAR(255)
);
GO


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


CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto NVARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    Descripcion NVARCHAR(255)
);
GO


CREATE TABLE Ventas (
    IdVenta INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente INT NOT NULL,
    Fecha DATETIME DEFAULT GETDATE(),
    Total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente)
);
GO


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


-- PROCEDIMIENTOS ALMACENADOS
--  Registrar una nueva cita
CREATE PROCEDURE sp_RegistrarCita
    @IdCliente INT,
    @IdEstilista INT,
    @IdServicio INT,
    @Fecha DATETIME
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Citas WHERE IdEstilista = @IdEstilista AND Fecha = @Fecha)
    BEGIN
        RAISERROR('El horario no está disponible.', 16, 1);
        RETURN;
    END

    INSERT INTO Citas (IdCliente, IdEstilista, IdServicio, Fecha, Estado)
    VALUES (@IdCliente, @IdEstilista, @IdServicio, @Fecha, 'Pendiente');
END;
GO

--  Actualizar estado de una cita
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

-- Registrar una venta
CREATE PROCEDURE sp_RegistrarVenta
    @IdCliente INT,
    @Total DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Ventas (IdCliente, Fecha, Total)
    VALUES (@IdCliente, GETDATE(), @Total);
END;
GO

--  Consultar historial de citas por cliente
CREATE PROCEDURE sp_ConsultarHistorialCliente
    @IdCliente INT
AS
BEGIN
    SELECT C.IdCita, S.NombreServicio, C.Fecha, C.Estado, E.Nombre AS Estilista
    FROM Citas C
    INNER JOIN Servicios S ON C.IdServicio = S.IdServicio
    INNER JOIN Estilistas E ON C.IdEstilista = E.IdEstilista
    WHERE C.IdCliente = @IdCliente
    ORDER BY C.Fecha DESC;
END;
GO

--  Verificar disponibilidad de estilista
CREATE PROCEDURE sp_VerificarDisponibilidad
    @IdEstilista INT,
    @Fecha DATETIME
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Citas WHERE IdEstilista = @IdEstilista AND Fecha = @Fecha)
        SELECT 'Ocupado' AS Estado;
    ELSE
        SELECT 'Disponible' AS Estado;
END;
GO
INSERT INTO Estilistas (Nombre, Especialidad, Telefono, Correo)
VALUES
('Emilia', 'Cabello', '6000-0001', 'emilia@matchasalon.com'),
('Carlos', 'Cabello', '6000-0002', 'carlos@matchasalon.com'),
('Antonella', 'Uñas', '6000-0003', 'antonella@matchasalon.com'),
('Sofía', 'Faciales', '6000-0004', 'sofia@matchasalon.com'),
('Diego', 'Faciales', '6000-0005', 'diego@matchasalon.com'),
('Ana', 'Maquillaje', '6000-0006', 'ana@matchasalon.com');


ALTER TABLE Clientes
ADD Contrasena NVARCHAR(255) NOT NULL DEFAULT('');

INSERT INTO Productos (NombreProducto, Precio, Stock, Descripcion)
VALUES
('Shampoo Herbal', 12.00, 20, 'Repara y fortalece desde el interior'),
('Acondicionador Nutrisoft', 14.00, 18, 'Hidratación profunda y brillo natural'),
('Cera de Cabello', 15.00, 25, 'Cera para estilizar tus peinados favoritos'),
('Gel para Cabello', 13.00, 30, 'Gel para definir rizos todo el día'),
('Aceite de Cabello', 28.00, 10, 'Reduce el frizz sin dejar grasa'),
('Plancha Profesional Titanium Pro', 39.99, 5, 'Plancha profesional de titanio'),
('Secadora', 39.99, 6, 'Secadora profesional de alta velocidad');
