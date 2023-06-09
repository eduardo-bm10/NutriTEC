CREATE TABLE ADMINISTRATOR (
   ID VARCHAR(9) PRIMARY KEY,
   FirstName VARCHAR(50) NOT NULL,
   LastName1 VARCHAR(50) NOT NULL,
   LastName2 VARCHAR(50) NOT NULL,
   Email VARCHAR(50) NOT NULL,
   Password VARCHAR(50) NOT NULL
);

CREATE TABLE NUTRITIONIST (
   ID VARCHAR(9) PRIMARY KEY,
   NutritionistCode VARCHAR(5) UNIQUE,
   FirstName VARCHAR(50) NOT NULL,
   LastName1 VARCHAR(50) NOT NULL,
   LastName2 VARCHAR(50) NOT NULL,
   Email VARCHAR(50) NOT NULL,
   Password VARCHAR(50) NOT NULL,
   Weight INTEGER NOT NULL,
   BMI FLOAT NOT NULL,
	Card_number varchar(16) NOT NULL,
   Address VARCHAR(50) NOT NULL,
   Photo VARCHAR(50) NOT NULL,
   PaymentID INT NOT NULL
);

CREATE TABLE PATIENT (
   ID VARCHAR(9) PRIMARY KEY,
   FirstName VARCHAR(50) NOT NULL,
   LastName1 VARCHAR(50) NOT NULL,
   LastName2 VARCHAR(50) NOT NULL,
   Email VARCHAR(50) NOT NULL,
   Password VARCHAR(50) NOT NULL,
   Weight INTEGER NOT NULL,
   BMI FLOAT NOT NULL,
   Address VARCHAR(50) NOT NULL,
   BirthDate DATE NOT NULL,
   Country VARCHAR(20) NOT NULL,
   MaxConsumption FLOAT NOT NULL
);

CREATE TABLE PLAN (
   ID SERIAL PRIMARY KEY,
   NutritionistID VARCHAR(50) NOT NULL,
   Description VARCHAR(50) NOT NULL
);

CREATE TABLE RECIPE (
   ID SERIAL PRIMARY KEY,
   Description VARCHAR(50) NOT NULL
);

CREATE TABLE PRODUCT (
   Barcode INTEGER PRIMARY KEY,
   Description VARCHAR(50) NOT NULL,
   Iron FLOAT NOT NULL,
   Sodium FLOAT NOT NULL,
   Energy FLOAT NOT NULL,
   Fat FLOAT NOT NULL,
   Calcium FLOAT NOT NULL,
   Carbohydrate FLOAT NOT NULL,
   Protein FLOAT NOT NULL,
   Status BOOLEAN NOT NULL
);

CREATE TABLE MEAL_TIME (
   ID SERIAL PRIMARY KEY,
   Name VARCHAR(50) NOT NULL
);

CREATE TABLE PAYMENT_TYPE (
   ID SERIAL PRIMARY KEY,
   Description VARCHAR(50) NOT NULL
);

CREATE TABLE CONSUMPTION (
   PatientID VARCHAR(9),
   Date DATE NOT NULL,
   MealTime INT NOT NULL,
   ProductBarcode INT NOT NULL,
	PRIMARY KEY (PatientID, Date, MealTime)
);

CREATE TABLE MEASUREMENT (
   PatientID VARCHAR(9),
   Date DATE NOT NULL,
   Waist FLOAT NOT NULL,
   Neck FLOAT NOT NULL,
   Hips FLOAT NOT NULL,
   MusclePercentage FLOAT NOT NULL,
   FatPercentage FLOAT NOT NULL,
   PRIMARY KEY (PatientID, Date)
);

CREATE TABLE ADMIN_PRODUCT_ASSOCIATION (
   AdminID VARCHAR(9),
   ProductBarcode INTEGER,
   Filler SERIAL,
   PRIMARY KEY (AdminID, ProductBarcode)
);

CREATE TABLE PATIENT_NUTRITIONIST_ASSOCIATION (
   NutritionistID VARCHAR(9),
   PatientID VARCHAR(9),
   Filler SERIAL,
   PRIMARY KEY (NutritionistID, PatientID)
);

CREATE TABLE RECIPE_PRODUCT_ASSOCIATION (
   RecipeID INT NOT NULL,
   ProductBarcode INT NOT NULL,
   ProductPortion INT NOT NULL,
   Filler SERIAL,
   PRIMARY KEY (RecipeID, ProductBarcode)
);

--- Plan ---
CREATE TABLE PLAN_MEALTIME_ASSOCIATION (
   PlanID INT,
   MealTimeID INT,
   ProductBarcode INT,
   Filler SERIAL,
   PRIMARY KEY (PlanID, MealTimeID, ProductBarcode)
);

CREATE TABLE PLAN_PATIENT_ASSOCIATION (
   PatientID VARCHAR(9),
   PlanID INT,
   StartDate DATE NOT NULL,
   EndDate DATE NOT NULL,
   Filler SERIAL,
   PRIMARY KEY (PatientID, PlanID)
);

CREATE TABLE VITAMINS (
	Product_barcode INT,
	Vitamin VARCHAR(20),
	PRIMARY KEY (Product_barcode, Vitamin)
);

ALTER TABLE NUTRITIONIST
ADD CONSTRAINT keys1
FOREIGN KEY (PaymentID)
REFERENCES PAYMENT_TYPE (ID);

ALTER TABLE PLAN
ADD CONSTRAINT keys2
FOREIGN KEY (NutritionistID)
REFERENCES NUTRITIONIST (ID);

ALTER TABLE CONSUMPTION
ADD CONSTRAINT keys3
FOREIGN KEY (PatientID)
REFERENCES PATIENT (ID);

ALTER TABLE CONSUMPTION
ADD CONSTRAINT keys4
FOREIGN KEY (MealTime)
REFERENCES MEAL_TIME (ID);

ALTER TABLE MEASUREMENT
ADD CONSTRAINT keys5
FOREIGN KEY (PatientID)
REFERENCES PATIENT (ID);

ALTER TABLE ADMIN_PRODUCT_ASSOCIATION
ADD CONSTRAINT keys6
FOREIGN KEY (AdminID)
REFERENCES ADMINISTRATOR (ID);

ALTER TABLE ADMIN_PRODUCT_ASSOCIATION
ADD CONSTRAINT keys7
FOREIGN KEY (ProductBarcode)
REFERENCES PRODUCT (Barcode);

ALTER TABLE PATIENT_NUTRITIONIST_ASSOCIATION
ADD CONSTRAINT keys8
FOREIGN KEY (NutritionistID)
REFERENCES NUTRITIONIST (ID);

ALTER TABLE PATIENT_NUTRITIONIST_ASSOCIATION
ADD CONSTRAINT keys9
FOREIGN KEY (PatientID)
REFERENCES PATIENT (ID);

ALTER TABLE RECIPE_PRODUCT_ASSOCIATION
ADD CONSTRAINT keys10
FOREIGN KEY (RecipeID)
REFERENCES RECIPE (ID);

ALTER TABLE RECIPE_PRODUCT_ASSOCIATION
ADD CONSTRAINT keys11
FOREIGN KEY (ProductBarcode)
REFERENCES PRODUCT (Barcode);

ALTER TABLE PLAN_MEALTIME_ASSOCIATION
ADD CONSTRAINT keys12
FOREIGN KEY (PlanID)
REFERENCES PLAN (ID);

ALTER TABLE PLAN_MEALTIME_ASSOCIATION
ADD CONSTRAINT keys13
FOREIGN KEY (MealTimeID)
REFERENCES MEAL_TIME (ID);

ALTER TABLE CONSUMPTION
ADD CONSTRAINT keys15
FOREIGN KEY (ProductBarcode)
REFERENCES PRODUCT (Barcode);

ALTER TABLE PLAN_PATIENT_ASSOCIATION
ADD CONSTRAINT keys16
FOREIGN KEY (PatientID)
REFERENCES PATIENT (ID);

ALTER TABLE PLAN_PATIENT_ASSOCIATION
ADD CONSTRAINT keys17
FOREIGN KEY (PlanID)
REFERENCES PLAN (ID);

ALTER TABLE VITAMINS
ADD CONSTRAINT keys18
FOREIGN KEY (Product_barcode)
REFERENCES PRODUCT (Barcode);

ALTER TABLE PLAN_MEALTIME_ASSOCIATION
ADD CONSTRAINT keys19
FOREIGN KEY (ProductBarcode)
REFERENCES PRODUCT (Barcode);