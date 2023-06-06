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
	FinalPayment INT
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

drop function calculate_payment
select * from calculate_payment(2)

SELECT * FROM NUTRITIONIST

-- Store Procedure: CUSTOMERS ADVANCE REPORT.
-- Description: Muestras las medidas registradas por un específico usuario dentro de un lapso indicado.
-- Parameters: SSN, StartDate, FinalDate
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE PROCEDURE customers_advance_report
(
	ssn INT, 
	startDate DATE, 
	finalDate DATE
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
		IF ssn = PatientID THEN
			p_exists := TRUE;
		END IF;
	END LOOP;
	IF p_exists THEN
		SELECT 
			PatientID, 
			Date, 
			Waist, 
			Neck, 
			Hips, 
			MusclePercentage, 
			FatPercentage
		FROM 
			MEASUREMENT 
		WHERE PatientID = ssn AND Date >= startDate AND Date <= finalDate;
	ELSE
		RAISE NOTICE 'Patient with SSN % does not exist', ssn;
	END IF;
END; $$

CALL calculate_payment(1)

-- Store Procedure: CREATE RECIPE.
-- Description: Verifica si los ingredientes existen para luego insertar a la base de datos
-- una nueva receta con dichos ingredientes.
-- Parameters: Description, Ingredients, Portions
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE PROCEDURE create_recipe
(
	description TEXT,
	ingredients TEXT[][] -- This syntax is [['IngredientName1', 'Portion1'], ['IngredientName2', 'Portion2'], etc...]
)
LANGUAGE PLPGSQL 
AS $$
DECLARE
	r_row RECORD;
	i TEXT[];
	counter INT := 0;
BEGIN
	FOR r_row IN (SELECT Description FROM Product) 
	LOOP
		IF ingredients[counter] = Description THEN
			counter := counter + 1;
			CONTINUE;
		ELSE
			RAISE NOTICE 'PRODUCT % DOES NOT EXIST', ingredients[counter];
			EXIT;
		END IF; 
	END LOOP;
	INSERT INTO RECIPE VALUES (DEFAULT, description);
	FOREACH i IN ARRAY ingredients 
	LOOP
		INSERT INTO RECIPE_PRODUCT_ASSOCIATION VALUES (DEFAULT, i[1], CAST(i[2] AS INT), DEFAULT);
	END LOOP;
END; $$

-- View: CaloriesPerMealTimeOnPlan
CREATE VIEW CaloriesPerMealTimeOnPlan AS
	SELECT 
		PLAN.ID AS PlanId, 
		PLAN.Description AS PlanDescription, 
		MEAL_TIME.Name AS Mealtime, 
		SUM(9*PRODUCT.Fat+4*PRODUCT.Carbohydrate+4*PRODUCT.Protein) AS TotalCalories
	FROM 
		PLAN JOIN PLAN_MEALTIME_ASSOCIATION ON PLAN.ID = PlanID JOIN
		MEAL_TIME ON MealTimeID = MEAL_TIME.ID JOIN
		MEALTIME_PRODUCT ON MEAL_TIME.ID = MEALTIME_PRODUCT.MealtimeID JOIN
		PRODUCT ON Product_barcode = PRODUCT.Barcode
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
		ProductPortion,
		(9*Fat + 4*Carbohydrate + 4*Protein)*ProductPortion as TotalProductCalories
	FROM
		RECIPE JOIN RECIPE_PRODUCT_ASSOCIATION ON RECIPE.ID = RecipeID JOIN
		PRODUCT ON ProductBarcode = Barcode;
		
-- View: PatientCurrentMeasures
CREATE VIEW NonAssociatedClients AS
	SELECT
		PATIENT.ID AS PatientSSN,
		CONCAT (PATIENT.FirstName, ' ', PATIENT.LastName1, ' ', PATIENT.LastName2) AS PatientName,
		AGE(NOW(), PATIENT.BirthDate) AS Age
	FROM
		PATIENT LEFT JOIN PATIENT_NUTRITIONIST_ASSOCIATION ON ID = PatientID
	WHERE PatientID IS NULL;



