CREATE TABLE ADMINISTRADOR(
   Cedula VARCHAR (9) primary KEY,
   Nombre VARCHAR (50) not null,
   Apellido1 VARCHAR (50) not null,
   Apellido2 VARCHAR (50) not null,
   Email VARCHAR (50) not null,
   Contrasena VARCHAR (50) not null
);

CREATE TABLE NUTRICIONISTA(
   Cedula VARCHAR (9) primary KEY,
   Codigo_Nutricionista VARCHAR(5) unique,
   Nombre VARCHAR (50) NOT null,
   Apellido1 VARCHAR (50) not null,
   Apellido2 VARCHAR (50) not null,
   Email VARCHAR (50) not null,
   Contrasena VARCHAR (50) not null,
   Peso INTEGER not null,
   IMC FLOAT not null,
   Direccion VARCHAR (50) not null,
   Foto BYTEA not null
);

CREATE TABLE PACIENTE(
   Cedula VARCHAR (9) primary KEY,
   Nombre VARCHAR (50) NOT null,
   Apellido1 VARCHAR (50) not null,
   Apellido2 VARCHAR (50) not null,
   Email VARCHAR (50) not null,
   Contrasena VARCHAR (50) not null,
   Peso INTEGER not null,
   IMC FLOAT not null,
   Direccion VARCHAR (50) not null,
   Fecha_Nacimiento DATE not null,
   Pais VARCHAR(20) not null,
   Consumo_Maximo FLOAT not null
);

create table PLAN(
	ID serial unique not null,
	Cedula_Nutri VARCHAR(50) unique not null,
	Descripcion VARCHAR(50) unique not null,
	primary key (ID, Cedula_Nutri)
);

create table RECETA(
	ID serial primary key,
	Descripcion VARCHAR(50) not null,
	Porciones VARCHAR(50) not null
);

create table PRODUCTO(
	Codigo_Barras INTEGER primary key,
	Descripcion VARCHAR(50) not null,
	Hierro FLOAT not null,
	Sodio FLOAT not null,
	Energia FLOAT not null,
	Grasa FLOAT not null,
	Calcio FLOAT not null,
	Carbohidrato FLOAT not null,
	Proteina FLOAT not null,
	Estado Boolean not null
);

create table TIEMPO_COMIDA(
	ID serial primary key,
	Nombre VARCHAR(50) not null
);

create table TIPO_COBRO(
	ID serial primary key,
	Cedula_Nutri VARCHAR(9) not null,
	Descripcion VARCHAR(50) not null
);

create table CONSUMO(
	Cedula_Paciente VARCHAR(9) primary key,
	Fecha DATE not null,
	Tiempo_Comida int not null
);

create table MEDIDAS(
	Cedula_Paciente VARCHAR(9) primary key,
	Fecha DATE not null,
	Cintura FLOAT not null,
	Cuello FLOAT not null,
	Caderas FLOAT not null,
	Porcentaje_Musculo FLOAT not null,
	Porcentaje_Grasa FLOAT not null
);

create table ASOCIACION_ADMIN_PRODUCTO(
	Cedula_Admin VARCHAR(9),
	Codigo_Barras_Producto INTEGER,
	primary key (Cedula_Admin, Codigo_Barras_Producto)
);

create table ASOCIACION_PACIENTE_NUTRI(
	Cedula_Nutri VARCHAR(9),
	Cedula_Paciente VARCHAR(9),
	primary key (Cedula_Nutri, Cedula_Paciente)
);

create table ASOCIACION_RECETA_PRODUCTO(
	ID_Receta INT not null, 
	Codigo_Barras_Producto INT not null,
	Porcion_Producto INT not null,
	primary key (ID_Receta, Codigo_Barras_Producto)
);

create table ASOCIACION_PLAN_TIEMPOCOMIDA(
	ID_Plan int,
	ID_Tiempo_Comida int,
	primary key (ID_Plan, ID_Tiempo_Comida)
);

create table ASOCIACION_PLAN_PACIENTE(
	Cedula_Paciente VARCHAR(9),
	ID_Plan int,
	Fecha_Inicio DATE not null,
	Fecha_Fin DATE not null,
	primary key (Cedula_Paciente, ID_Plan)
);

alter table PLAN
add constraint llaves1
foreign key (Cedula_Nutri)
references NUTRICIONISTA (Cedula);

alter table TIPO_COBRO
add constraint llaves2
foreign key (Cedula_Nutri)
references NUTRICIONISTA (Cedula);

alter table CONSUMO
add constraint llaves3
foreign key (Cedula_Paciente)
references PACIENTE (Cedula);

alter table CONSUMO
add constraint llaves4
foreign key (Tiempo_Comida)
references TIEMPO_COMIDA (ID);

alter table MEDIDAS
add constraint llaves5
foreign key (Cedula_Paciente)
references PACIENTE (Cedula);
 
alter table ASOCIACION_ADMIN_PRODUCTO
add constraint llaves6
foreign key (Cedula_Admin)
references ADMINISTRADOR (Cedula);

alter table ASOCIACION_ADMIN_PRODUCTO
add constraint llaves7
foreign key (Codigo_Barras_Producto)
references PRODUCTO (Codigo_Barras);

alter table  ASOCIACION_PACIENTE_NUTRI
add constraint llaves8
foreign key (Cedula_Nutri)
references NUTRICIONISTA (Cedula);

alter table  ASOCIACION_PACIENTE_NUTRI
add constraint llaves9
foreign key (Cedula_Paciente)
references PACIENTE (Cedula);

alter table ASOCIACION_RECETA_PRODUCTO
add constraint llaves10
foreign key (ID_Receta)
references RECETA (ID);

alter table ASOCIACION_RECETA_PRODUCTO
add constraint llaves11
foreign key (Codigo_Barras_Producto)
references PRODUCTO (Codigo_Barras);

alter table ASOCIACION_PLAN_TIEMPOCOMIDA
add constraint llaves12
foreign key (ID_Plan)
references PLAN (ID);

alter table ASOCIACION_PLAN_TIEMPOCOMIDA
add constraint llaves13
foreign key (ID_Tiempo_Comida)
references PLAN (ID);

alter table ASOCIACION_PLAN_PACIENTE
add constraint llaves14
foreign key (Cedula_Paciente)
references PACIENTE (Cedula);

alter table ASOCIACION_PLAN_PACIENTE
add constraint llaves15
foreign key (ID_Plan)
references PLAN (ID);