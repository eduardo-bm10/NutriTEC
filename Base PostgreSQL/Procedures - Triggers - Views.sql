-- Función: CALCULATE PAYMENT.
-- Description: Calcula el pago que debe realizarse a los nutricionistas
-- de acuerdo con su modo de pago.
-- Parameters: Payment
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE FUNCTION calculate_payment(p_payment INT)
RETURNS TABLE (
	Email VARCHAR,
	FullName VARCHAR,
	TotalPayment INT,
	Discount VARCHAR,
	FinalPayment REAL
)
LANGUAGE PLPGSQL AS $$
DECLARE
	var_r RECORD;
BEGIN
	FOR var_r IN (SELECT 
					NUTRITIONIST.Email AS NutriEmail, 
					CONCAT(FirstName,' ',LastName1,' ',LastName2) AS FullName,
					COUNT(PatientID) AS TotalAmount,
				  	CASE
				  		WHEN PaymentID = 1 THEN 'N/A'
				  		WHEN PaymentID = 2 THEN '5%'
				  		WHEN PaymentID = 3 THEN '10%'
				  	END AS Discount,
					CASE
						WHEN PaymentID = 1 THEN COUNT(PatientID)
						WHEN PaymentID = 2 THEN COUNT(PatientID)-(COUNT(PatientID)*0.05)
						WHEN PaymentID = 3 THEN COUNT(PatientID)-(COUNT(PatientID)*0.10)
					END AS FinalAmount
				FROM 
					NUTRITIONIST LEFT JOIN PATIENT_NUTRITIONIST_ASSOCIATION ON ID = NutritionistID
				WHERE PaymentID = p_payment
				GROUP BY NUTRITIONIST.Email, FirstName, LastName1, LastName2, PaymentID)
	LOOP
		Email := var_r.NutriEmail;
		FullName := var_r.FullName;
		TotalPayment := var_r.TotalAmount;
		Discount := var_r.Discount;
		FinalPayment := var_r.FinalAmount;
		RETURN NEXT;
	END LOOP;
END; $$

-- Store Procedure: CUSTOMERS ADVANCE REPORT.
-- Description: Muestras las medidas registradas por un específico usuario dentro de un lapso indicado.
-- Parameters: SSN, StartDate, FinalDate
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE FUNCTION customers_advance_report
(
	ssn VARCHAR(9), 
	startDate DATE, 
	finalDate DATE
)
RETURNS TABLE (
	Patient INT,
	ReDate DATE,
	Waist REAL,
	Neck REAL,
	Hips REAL,
	MusclePercentage REAL,
	FatPercentage REAL
)
LANGUAGE PLPGSQL AS $$
DECLARE
	r_row RECORD;
	p_exists BOOLEAN := FALSE;
BEGIN
	FOR r_row IN (	SELECT PatientID
					FROM 
					MEASUREMENT)
	LOOP
		IF ssn = r_row.PatientID THEN
			p_exists := TRUE;
		END IF;
	END LOOP;
	IF p_exists THEN
		FOR r_row IN (SELECT 
							PatientID, 
							Date, 
							Waist, 
							Neck, 
							Hips, 
							MusclePercentage, 
							FatPercentage
						FROM 
							MEASUREMENT 
						WHERE PatientID = ssn AND Date >= startDate AND Date <= finalDate)
		LOOP
			Patient := r_row.PatientID;
			ReDate := r_row.Date;
			Waist := r_row.Waist;
			Neck := r_row.Neck;
			Hips := r_row.Hips;
			MusclePercentage := r_row.MusclePercentage;
			FatPercentage := r_row.FatPercentage;
			RETURN NEXT;
		END LOOP;
	ELSE
		RAISE NOTICE 'Patient with SSN % does not exist', ssn;
	END IF;
END; $$

drop function customers_advance_report

select * from customers_advance_report('111278965', '2023-05-05', '2023-06-06')

-- Store Procedure: CREATE RECIPE.
-- Description: Verifica si los ingredientes existen para luego insertar a la base de datos
-- una nueva receta con dichos ingredientes.
-- Parameters: Description, Ingredients, Portions
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE PROCEDURE create_recipe
(
	description VARCHAR,
	ingredients INT[],
	portions INT[]
)
LANGUAGE PLPGSQL 
AS $$
DECLARE
	r_row RECORD;
	i INT;
	counter1 INT := 1;
	counter2 INT := 1;
BEGIN
	FOR r_row IN (SELECT Barcode FROM Product) 
	LOOP
		IF ingredients[counter1] = r_row.Barcode THEN
			counter1 := counter1 + 1;
			CONTINUE;
		ELSE
			RAISE NOTICE 'PRODUCT % DOES NOT EXIST', ingredients[counter1];
			EXIT;
		END IF; 
	END LOOP;
	INSERT INTO RECIPE VALUES (DEFAULT, description);
	FOREACH i IN ARRAY ingredients 
	LOOP
		INSERT INTO RECIPE_PRODUCT_ASSOCIATION (RecipeId, ProductBarcode, ProductPortion) SELECT MAX(ID), i, portions[counter2] FROM RECIPE;
		counter2 := counter2 + 1;
	END LOOP;
END; $$

call create_recipe('Gallo Pinto', '{7,9,10}', '{100,4,9}')

select * from product
-- View: CaloriesPerMealTimeOnPlan
CREATE VIEW CaloriesPerMealTimeOnPlan AS
	SELECT 
		PLAN.ID AS PlanId,
		MEAL_TIME.Name AS Mealtime, 
		SUM(9*PRODUCT.Fat+4*PRODUCT.Carbohydrate+4*PRODUCT.Protein) AS TotalCalories
	FROM 
		PLAN JOIN PLAN_MEALTIME_ASSOCIATION ON PLAN.ID = PlanID JOIN
		MEAL_TIME ON MealTimeID = MEAL_TIME.ID JOIN
		PRODUCT ON ProductBarcode = PRODUCT.Barcode
	GROUP BY 
		PLAN.ID, 
		PLAN.Description, 
		MEAL_TIME.Name;
		
-- View: TotalRecipeCalories
CREATE VIEW TotalRecipeCalories AS
	SELECT
		RECIPE.ID AS RecipeID,
		RECIPE.Description AS RecipeDescription,
		PRODUCT.Description AS Product,
		(9*Fat + 4*Carbohydrate + 4*Protein) AS ProductCalories,
		ProductPortion,
		(9*Fat + 4*Carbohydrate + 4*Protein)*ProductPortion AS TotalProductCalories,
		SUM((9*Fat + 4*Carbohydrate + 4*Protein)*ProductPortion) AS TotalRecipeCalories
	FROM
		RECIPE JOIN RECIPE_PRODUCT_ASSOCIATION ON RECIPE.ID = RecipeID JOIN
		PRODUCT ON ProductBarcode = Barcode
	GROUP BY RECIPE.ID, RECIPE.DESCRIPTION, PRODUCT.DESCRIPTION, ProductPortion, Fat, Carbohydrate, Protein
	ORDER BY RECIPE.DESCRIPTION;
drop view totalrecipecalories
select * from TotalRecipeCalories

-- View: PatientCurrentMeasures
CREATE VIEW NonAssociatedClients AS
	SELECT
		PATIENT.ID AS PatientSSN,
		CONCAT (PATIENT.FirstName, ' ', PATIENT.LastName1, ' ', PATIENT.LastName2) AS PatientName,
		AGE(PATIENT.BirthDate, NOW()) AS Age
	FROM
		PATIENT LEFT JOIN PATIENT_NUTRITIONIST_ASSOCIATION ON ID = PatientID
	WHERE PatientID IS NULL;

drop view nonassociatedclients
select * from nonassociatedclients