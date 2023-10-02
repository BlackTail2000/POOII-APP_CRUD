Use Master
If DB_ID('BASE_DATOS') Is Not Null
Drop Database BASE_DATOS
Go

Create Database BASE_DATOS
Go

Use BASE_DATOS
Go

Create Table DEPARTAMENTO(
IDDEPARTAMENTO Int Primary Key Identity,
NOM_DEPARTAMENTO Varchar(50))
Go

Create Table PERSONAL(
IDPERSONAL Int Primary Key Identity,
NOM_PER Varchar(50),
IDDEPARTAMENTO Int Not Null Foreign Key References DEPARTAMENTO,
SUELDO Decimal(9,2),
FEC_CON Date,
EST_PER Char(1))
Go

Insert Into DEPARTAMENTO
Values ('Administración'),
       ('Contabilidad'),
	   ('Marketing'),
	   ('Sistemas'),
	   ('Ventas')
Go

Select * From DEPARTAMENTO
Go

Insert Into PERSONAL
Values ('Juan Carlos Alva Castro', 1, 4400, '20001010', 'A'),
       ('Marco Antonio Medina Gonzáles', 4, 3500, '19980506', 'A'),
	   ('Ana María Herrera Díaz', 2, 3000, '20011107', 'A')
Go

Select * From PERSONAL
Go

--- Procedimientos Almacenados
Create Procedure pa_listaDpartamentos
As
Select * From DEPARTAMENTO
Go

Create Procedure pa_listaPersonal
As
Set DateFormat DMY
Select P.IDPERSONAL, P.NOM_PER, D.NOM_DEPARTAMENTO, D.IDDEPARTAMENTO, P.SUELDO, CONVERT(CHAR(10), P.FEC_CON, 103) 'FEC_CON'
From PERSONAL P
Inner Join DEPARTAMENTO D On P.IDDEPARTAMENTO = D.IDDEPARTAMENTO
Where P.EST_PER = 'A'
Go

Execute pa_listaPersonal
Go

Create Procedure pa_nuevoPersonal
@nombre Varchar(50),
@iddep Int,
@sueldo Decimal(9,2),
@fecha Varchar(10),
@est Char(1)
As
Set DateFormat DMY
Insert Into Personal
Values (@nombre, @iddep, @sueldo, Convert(Date, @fecha), @est)
Go

Create Procedure pa_modificaPersonal
@idper Int,
@nombre Varchar(50),
@iddep Int,
@sueldo Decimal(9,2),
@fecha Varchar(10),
@est Char(1)
As
Set DateFormat DMY
Update PERSONAL
Set NOM_PER = @nombre, IDDEPARTAMENTO = @iddep, SUELDO = @sueldo,
    FEC_CON = Convert(Date, @fecha), EST_PER = @est
	Where IDPERSONAL = @idper
Go

Create Procedure pa_eliminaPersonal
@idper Int
As
Update Personal
Set EST_PER = 'E'
Where IDPERSONAL = @idper
Go