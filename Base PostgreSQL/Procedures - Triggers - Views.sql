-- funtion: CALCULATE_PAYMENT.
-- summary: Calculates the corresponding amount of money that a nutritionist must receive.
-- param p_payment: The payment type.
-- returns: a table containing email, full name, and total payment that corresponds to a nutritionist.
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

-- function: CUSTOMERS_ADVANCE_REPORT.
-- summary: shows the measurements registered by a specific user within an indicated period.
-- param ssn: the ssn of the user. 
-- param startDate: the initial date of the period.
-- param finalDate: the final date of the period.
-- returns: table containing patientId, date of the register, and varios measures.
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
	FOR r_row IN (SELECT PatientID FROM MEASUREMENT)
	LOOP
		IF ssn = r_row.PatientID THEN
			p_exists := TRUE;
		END IF;
	END LOOP;
	IF p_exists THEN
		FOR r_row IN (SELECT 
							PatientID, 
							Date, 
							MEASUREMENT.Waist AS W, 
							MEASUREMENT.Neck AS N, 
							MEASUREMENT.Hips AS H, 
							MEASUREMENT.MusclePercentage AS M, 
							MEASUREMENT.FatPercentage AS F
						FROM 
							MEASUREMENT 
						WHERE PatientID = ssn AND Date >= startDate AND Date <= finalDate
					 	ORDER BY Date)
		LOOP
			Patient := r_row.PatientID;
			ReDate := r_row.Date;
			Waist := r_row.W;
			Neck := r_row.N;
			Hips := r_row.H;
			MusclePercentage := r_row.M;
			FatPercentage := r_row.F;
			RETURN NEXT;
		END LOOP;
	ELSE
		RAISE NOTICE 'Patient with SSN % does not exist', ssn;
	END IF;
END; $$

-- sp: CREATE_RECIPE.
-- summary: verifies if the ingredients exist and then inserts a new recipe with these ingredients into the database.
-- param description: the description of the new recipe to be created.
-- param ingredients: a list of products to be used in the recipe.
-- param portions: a list of portions for each product.
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

-- function: vitamin_per_product
-- summary: retrieves all the vitamins contained in a specified product.
-- param p_barcode: the barcode of the specified product.
-- returns: table containing all vitamins of that product.
-- Author: Eduardo Bolívar Minguet
CREATE OR REPLACE FUNCTION vitamins_per_product(p_barcode INT)
RETURNS TABLE (
	Vitamin VARCHAR(20)
)
LANGUAGE PLPGSQL
AS $$
DECLARE
	r_row1 RECORD;
	r_row2 RECORD;
	exist BOOLEAN := FALSE;
BEGIN
	FOR r_row1 IN (SELECT Barcode FROM PRODUCT)
	LOOP
		IF r_row1.Barcode = p_barcode THEN
			exist := TRUE;
		ELSE
			CONTINUE;
		END IF;
	END LOOP;
	IF exist THEN
		FOR r_row2 IN (SELECT VITAMINS.Vitamin AS V FROM VITAMINS WHERE Product_barcode = p_barcode)
				LOOP
					Vitamin := r_row2.V;
					RETURN NEXT;
				END LOOP;
	ELSE
		RAISE NOTICE 'PRODUCT % DOES NOT EXIST', p_barcode;
	END IF;
END; $$

-- view: CaloriesPerMealTimeOnPlan
-- summary: shows the total calories contained on a plan.
-- author: Eduardo Bolívar Minguet
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
		
-- view: TotalRecipeCalories
-- summary: shows the total calories on a recipe.
-- author: Eduardo Bolívar Minguet
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

-- view: PatientCurrentMeasures
-- summary: shows all the clients that are not associated to a nutritionist.
-- author: Eduardo Bolívar Minguet
CREATE VIEW NonAssociatedClients AS
	SELECT
		PATIENT.ID AS PatientSSN,
		CONCAT (PATIENT.FirstName, ' ', PATIENT.LastName1, ' ', PATIENT.LastName2) AS PatientName
	FROM
		PATIENT LEFT JOIN PATIENT_NUTRITIONIST_ASSOCIATION ON ID = PatientID
	WHERE PatientID IS NULL;