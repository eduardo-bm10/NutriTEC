CREATE PROCEDURE calculatePayment()
AS
BEGIN
	SELECT 
		Email, 
		CONCAT(Nombre,' ',Apellido1,' ',Apellido2) AS Nombre_completo,
		COUNT(Cedula_paciente) AS Monto_total,
		CASE 
			WHEN Id_cobro = 1 THEN 'N/A'
			WHEN Id_cobro = 2 THEN '5%'
			WHEN Id_cobro = 3 THEN '10%'
		END AS Descuento,
		CASE
			WHEN Id_cobro = 1 THEN COUNT(Cedula_paciente)
			WHEN Id_cobro = 2 THEN COUNT(Cedula_paciente)-(COUNT(Cedula_paciente)*0.05)
			WHEN Id_cobro = 2 THEN COUNT(Cedula_paciente)-(COUNT(Cedula_paciente)*0.10)
		END AS Monto_a_cobrar
		FROM 
		NUTRICIONISTA JOIN ASOCIACION_PACIENTE_NUTRI ON Cedula = Cedula_nutri
		GROUP BY Email, Nombre, Apellido1, Apellido2, Id_cobro
END

CREATE PROCEDURE reporteAvance(cedula INT, fecha_inicio DATE, fecha_fin DATE)
AS
BEGIN

