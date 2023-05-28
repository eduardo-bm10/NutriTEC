-- Store Procedure: CALCULATE PAYMENT.
-- Description: Calcula el pago que debe realizarse a los nutricionistas
-- de acuerdo con su modo de pago.
-- Parameters: Payment
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE PROCEDURE calculatePayment
(
	payment INT
)
LANGUAGE PLPGSQL AS
$$
DECLARE
	discount text
BEGIN
	IF payment = 1 THEN
		discount := 'N/A'
	ELSIF payment = 2 THEN
		discount  := '5%'
	ELSIF payment = 3 THEN
	 	discount := '10%'
	ELSE
		RAISE NOTICE 'INVALID PAYMENT ID';
	END IF
	SELECT 
		Email, 
		CONCAT(FirstName,' ',LastName1,' ',LastName2) AS FullName,
		COUNT(PatientID) AS TotalAmount,
		discount AS Discount,
		CASE
			WHEN PaymentID = 1 THEN COUNT(PatientID)
			WHEN PaymentID = 2 THEN COUNT(PatientID)-(COUNT(PatientID)*0.05)
			WHEN PaymentID = 2 THEN COUNT(PatientID)-(COUNT(PatientID)*0.10)
		END AS FinalAmount
		FROM 
		NUTRICIONIST JOIN PATIENT_NUTRITIONIST_ASSOCIATION ON ID = NutritionistID
		GROUP BY Email, FirstName, LastName1, LastName2, PaymentID
END 
$$

-- Store Procedure: CUSTOMERS ADVANCE REPORT.
-- Description: Muestras las medidas registradas por un específico usuario dentro de un lapso indicado.
-- Parameters: SSN, StartDate, FinalDate
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE PROCEDURE customersAdvanceReport(ssn INT, startDate DATE, finalDate DATE)
LANGUAGE PLPGSQL AS $$
BEGIN
	SELECT *
	FROM 
	MEASUREMENT WHERE startDate <= Date AND Date <= finalDate AND PatientID = ssn
END $$

-- Store Procedure: CREATE RECIPE.
-- Description: Verifica si los ingredientes existen para luego insertar a la base de datos
-- una nueva receta con dichos ingredientes.
-- Parameters: Description, Ingredients, Portions
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE PROCEDURE createRecipe
(
	description text,
	ingredients text[],
	portions int[]
)
LANGUAGE PLPGSQL AS
$$
DECLARE
	counter INT := 1
BEGIN
	FOR i IN ingredients LOOP
		IF i NOT IN SELECT Description FROM Product THEN
			RAISE NOTICE 'PRODUCT % DOES NOT EXIST', i
	END LOOP
	INSERT INTO RECIPE VALUES (DEFAULT, description)
	FOR i IN ingredients LOOP
		INSERT INTO RECIPE_PRODUCT_ASSOCIATION VALUES (DEFAULT, i, portions[counter], DEFAULT)
		counter := counter + 1
	END LOOP
END; $$




