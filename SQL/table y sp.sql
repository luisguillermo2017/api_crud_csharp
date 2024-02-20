create table T_Z_TEST (
id int identity(1,1) not null,
nombre varchar(50) not null,
apellido varchar(50) not null,
edad int not null,
fecha_nacimiento date not null,
fecha_hora_registro datetime not null,
estado bit not null

primary key(id)
)

--CRUD

--C
insert into T_Z_TEST
(nombre, apellido, edad, fecha_nacimiento, fecha_hora_registro, estado)
values
('Oscar', 'Castillo', 43, '1980/12/01', '2024/01/23 10:10:10', 1 )

create procedure SP_Z_TEST_CREATE(
@nombre varchar(50), @apellido varchar(50), @edad int,
@fecha_nacimiento date, @fecha_hora_registro datetime, @estado bit )
as
begin
set nocount on
insert into T_Z_TEST
(nombre, apellido, edad, fecha_nacimiento, fecha_hora_registro, estado)
values
(@nombre, @apellido, @edad, @fecha_nacimiento, @fecha_hora_registro, @estado )
end

exec SP_Z_TEST_CREATE @nombre = 'Angel', @apellido = 'Martinez', @edad = 20,
@fecha_nacimiento = '2004/12/01', @fecha_hora_registro = '2024/11/01 15:20:00', @estado = 1


--R
select * from T_Z_TEST

create view VIEW_Z_TEST_READ
as select * from T_Z_TEST

select v.id, v.nombre, v.apellido, v.fecha_nacimiento, v.fecha_hora_registro, v.estado
from VIEW_Z_TEST_READ v

create procedure SP_Z_TEST_READ_SIMPLE
as
begin
set nocount on
select v.id, v.nombre, v.apellido, v.fecha_nacimiento, v.fecha_hora_registro, v.estado
from VIEW_Z_TEST_READ v
for json path

SELECT 'success' AS MSJ_TIPO, 'Exito al realizar la acción.' AS MSJ_TEXT;  
end

exec SP_Z_TEST_READ_SIMPLE


--R con parametro
select * from T_Z_TEST where id = 5

create procedure SP_Z_TEST_READ
(@id int)
as
begin
set nocount on
select v.id, v.nombre, v.apellido, v.fecha_nacimiento, v.fecha_hora_registro, v.estado from VIEW_Z_TEST_READ v where v.id = @id
for json path, WITHOUT_ARRAY_WRAPPER

SELECT 'success' AS MSJ_TIPO, 'Exito al realizar la acción.' AS MSJ_TEXT;  
end

exec SP_Z_TEST_READ @id = 5

--U
update T_Z_TEST set 
nombre = 'Mario', apellido = 'Coto', edad = 45,
fecha_nacimiento = '1980/10/01', fecha_hora_registro = '2024/01/23 11:00:00', estado = 1
where id = 5

create procedure SP_Z_TEST_UPDATE
(@nombre nvarchar(50), @apellido nvarchar(50), @edad int,
@fecha_nacimiento date, @fecha_hora_registro datetime, @estado bit, @id int)
as
begin
set nocount on
update T_Z_TEST set 
nombre = @nombre, apellido = @apellido, edad = @edad,
fecha_nacimiento = @fecha_nacimiento, fecha_hora_registro = @fecha_hora_registro, estado = @estado
where id = @id
end

exec SP_Z_TEST_UPDATE @nombre = 'Martin', @apellido = 'Montero', @edad = 21,
@fecha_nacimiento = '2003/11/01', @fecha_hora_registro = '2024/11/15 19:20:00', @estado = 1,
@id = 6

--D
update T_Z_TEST set 
estado = 0
where id = 5

create procedure SP_Z_TEST_DELETE_DIGITAL
(@id int)
as
begin
set nocount on
update T_Z_TEST set 
estado = 0
where id = @id
end

exec SP_Z_TEST_DELETE_DIGITAL @id = 6

--C
exec SP_Z_TEST_CREATE @nombre = 'Viviana', @apellido = 'Martinez', @edad = 20,
@fecha_nacimiento = '2004/12/01', @fecha_hora_registro = '2024/11/01 15:20:00', @estado = 1

--R
exec SP_Z_TEST_READ_SIMPLE

--R con parametro
exec SP_Z_TEST_READ @nombre = 'Mario'

--U
exec SP_Z_TEST_UPDATE @nombre = 'Martin', @apellido = 'Montero', @edad = 21,
@fecha_nacimiento = '2003/11/01', @fecha_hora_registro = '2024/11/15 19:20:00', @estado = 1,
@id = 6

--D
exec SP_Z_TEST_DELETE_DIGITAL @id = 6


@nombre, @apellido, @edad, @fecha_nacimiento, @fecha_hora_registro, @estado, @id



--C

create procedure SP_Z_TEST_CREATE_UPDATE(
@id int, @nombre varchar(50), @apellido varchar(50), @edad int,
@fecha_nacimiento date, @fecha_hora_registro datetime, @estado bit )
as
begin

	DECLARE @MSJ_TIPO AS VARCHAR(50);
	DECLARE @MSJ_TEXT AS VARCHAR(200);

	set nocount on

	IF(@id > 0)
		begin

			update T_Z_TEST set 
			nombre = @nombre, apellido = @apellido, edad = @edad,
			fecha_nacimiento = @fecha_nacimiento, 
			fecha_hora_registro = @fecha_hora_registro, estado = @estado
			where id = @id

			SET @MSJ_TIPO = 'success';  SET @MSJ_TEXT = 'Registro actualizado.'; 

		end
	else
		begin
			IF((SELECT COUNT(*) FROM T_Z_TEST WHERE nombre = @nombre and apellido = @apellido ) <= 0)
				begin

					insert into T_Z_TEST
					(nombre, apellido, edad, fecha_nacimiento, fecha_hora_registro, estado)
					values
					(@nombre, @apellido, @edad, @fecha_nacimiento, @fecha_hora_registro, @estado )

					SET @MSJ_TIPO = 'success';  SET @MSJ_TEXT = 'Registro Correcto.'; 
			
				end
			ELSE
					BEGIN
						SET @MSJ_TIPO = 'warning';  SET @MSJ_TEXT = 'Ya existe un usuario con ese nombre y apellido.'; 
					END
		end

		SELECT @MSJ_TIPO AS MSJ_TIPO, @MSJ_TEXT AS MSJ_TEXT;   

end


exec SP_Z_TEST_CREATE_UPDATE @id = 8, @nombre = 'Angel Manuel',
@apellido = 'Martinez', @edad = 20,
@fecha_nacimiento = '2004/12/01',
@fecha_hora_registro = '2024/11/01 15:20:00',
@estado = 1

select * from T_Z_TEST
