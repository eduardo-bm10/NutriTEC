import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class GetApiService {

  private baseUrl = 'https://postgresqlapi.azurewebsites.net';

  constructor(private http: HttpClient, private router:Router) {}

  ruta(ruta:string){
    this.router.navigate([ruta]);
  }

  logout(){
    localStorage.removeItem('usuario');
    this.router.navigate(['/']);
  }

  // Iniciar sesión--------------------------------------------
  login(email: string, password: string) {
    const url = `${this.baseUrl}/api/Login/login?email=${email}&password=${password}`;
    return this.http.post(url, null);
  }
  // Administrador--------------------------------------------

  // Obtener todos los administradores
  getAllAdministrators() {
    const url = `${this.baseUrl}/api/Administrator`;
    return this.http.get(url);
  }

  // Obtener un administrador por ID
  getAdministratorById(id: string) {
    const url = `${this.baseUrl}/api/Administrator/${id}`;
    return this.http.get(url);
  }

  // Crear un nuevo administrador (Registro)
  createAdministrator(id: string, firstname: string, lastname1: string, lastname2: string, email: string, password: string) {
    const url = `${this.baseUrl}/api/Administrator/${id}?firstname=${firstname}&lastname1=${lastname1}&lastname2=${lastname2}&email=${email}&password=${password}`;
    return this.http.post(url, null);
  }

  // Actualizar un administrador
  updateAdministrator(id: string, firstname: string, lastname1: string, lastname2: string, email: string, password: string) {
    const url = `${this.baseUrl}/api/Administrator/${id}?firstname=${firstname}&lastname1=${lastname1}&lastname2=${lastname2}&email=${email}&password=${password}`;
    return this.http.put(url, null);
  }

  // Eliminar un administrador
  deleteAdministrator(id: string) {
    const url = `${this.baseUrl}/api/Administrator/${id}`;
    return this.http.delete(url);
  }

  // Nutritionist--------------------------------------------
  // Obtener lista de nutricionistas
  getNutritionists() {
    const url = `${this.baseUrl}/api/Nutritionists`;
    return this.http.get(url);
  }

  // Crear un nuevo nutricionista
  createNutritionist(
    id: string,
    nutritionistcode: string,
    firstname: string,
    lastname1: string,
    lastname2: string,
    email: string,
    password: string,
    weight: number,
    bmi: number,
    creditCard: number,
    address: string,
    paymentid: number,
    photo: string
  ) {
    const url = `${this.baseUrl}/api/Nutritionists?id=${id}&nutritionistcode=${nutritionistcode}&firstname=${firstname}&lastname1=${lastname1}&lastname2=${lastname2}&email=${email}&password=${password}&weight=${weight}&bmi=${bmi}&creditCard=${creditCard}&address=${address}&paymentid=${paymentid}&photo=${photo}`;

    return this.http.post(url, null, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  // Obtener un nutricionista por su ID
  getNutritionistById(id: string) {
    const url = `${this.baseUrl}/api/Nutritionists/${id}`;
    return this.http.get(url);
  }

  // Actualizar un nutricionista
  updateNutritionist(
    id: string,
    nutritionistcode: string,
    firstname: string,
    lastname1: string,
    lastname2: string,
    email: string,
    password: string,
    weight: number,
    bmi: number,
    address: string,
    paymentid: number,
    photo: string
  ) {
    const url = `${this.baseUrl}/api/Nutritionists/${id}?nutritionistcode=${nutritionistcode}&firstname=${firstname}&lastname1=${lastname1}&lastname2=${lastname2}&email=${email}&password=${password}&weight=${weight}&bmi=${bmi}&address=${address}&paymentid=${paymentid}&photo=${photo}`;

    return this.http.put(url, null, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  // Eliminar un nutricionista
  deleteNutritionist(id: string) {
    const url = `${this.baseUrl}/api/Nutritionists/${id}`;
    return this.http.delete(url);
  }

  // Patient--------------------------------------------
  // Obtener lista de pacientes
  getPatients() {
    const url = `${this.baseUrl}/api/Patients`;
    return this.http.get(url);
  }

  // Crear un nuevo paciente
  createPatient(
    id: string,
    firstname: string,
    lastname1: string,
    lastname2: string,
    email: string,
    password: string,
    weight: number,
    bmi: number,
    address: string,
    birthdate: string,
    country: string,
    maxconsumption: number,
    waist: number,
    neck: number,
    hips: number,
    musclePercentage: number,
    fatPercentage: number
  ) {
    const url = `${this.baseUrl}/api/Patients?id=${id}&firstname=${firstname}&lastname1=${lastname1}&lastname2=${lastname2}&email=${email}&password=${password}&weight=${weight}&bmi=${bmi}&address=${address}&birthdate=${birthdate}&country=${country}&maxconsumption=${maxconsumption}&waist=${waist}&neck=${neck}&hips=${hips}&musclePercentage=${musclePercentage}&fatPercentage=${fatPercentage}`;

    return this.http.post(url, null, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  // Obtener un paciente por su ID
  getPatientById(id: string) {
    const url = `${this.baseUrl}/api/Patients/${id}`;
    return this.http.get(url);
  }

  // Actualizar un paciente
  updatePatient(
    id: string,
    firstname: string,
    lastname1: string,
    lastname2: string,
    email: string,
    password: string,
    weight: number,
    bmi: number,
    address: string,
    birthdate: string,
    country: string,
    maxconsumption: number
  ) {
    const url = `${this.baseUrl}/api/Patients/${id}?firstname=${firstname}&lastname1=${lastname1}&lastname2=${lastname2}&email=${email}&password=${password}&weight=${weight}&bmi=${bmi}&address=${address}&birthdate=${birthdate}&country=${country}&maxconsumption=${maxconsumption}`;

    return this.http.put(url, null, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  // Eliminar un paciente
  deletePatient(id: string) {
    const url = `${this.baseUrl}/api/Patients/${id}`;
    return this.http.delete(url);
  }

  // Consumption--------------------------------------------
  createConsumption(
    patientId: string,
    date: Date,
    mealtimeId: number,
    productBarcode: number
  ) {
    const url = `${this.baseUrl}/api/Consumptions?patientId=${patientId}&date=${date}&mealtimeId=${mealtimeId}&productBarcode=${productBarcode}`;
    return this.http.post(url, null, {});
  }


  // Measurements--------------------------------------------

  getMeasurements() {
    return this.http.get(`${this.baseUrl}/api/Measurements`);
  }

  createMeasurement(
    patientId: string,
    waist: number,
    neck: number,
    hips: number,
    musclePercentage: number,
    fatPercentage: number
  ) {
    const url = `${this.baseUrl}/api/Measurements/${patientId}?waist=${waist}&neck=${neck}&hips=${hips}&musclePercentage=${musclePercentage}&fatPercentage=${fatPercentage}`;
    return this.http.post(url, {});
  }

  getMeasurementById(id: number) {
    const url = `${this.baseUrl}/api/Measurements/${id}`;
    return this.http.get(url);
  }

  updateMeasurement(
    id: number,
    waist: number,
    neck: number,
    hips: number,
    musclePercentage: number,
    fatPercentage: number
  ) {
    const url = `${this.baseUrl}/api/Measurements/${id}?waist=${waist}&neck=${neck}&hips=${hips}&musclePercentage=${musclePercentage}&fatPercentage=${fatPercentage}`;
    return this.http.put(url, {});
  }

  deleteMeasurement(id: number) {
    const url = `${this.baseUrl}/api/Measurements/${id}`;
    return this.http.delete(url);
  }

  // PaymentType--------------------------------------------

  getPaymentTypes() {
    const url = `${this.baseUrl}/api/PaymentType`;
    return this.http.get(url);
  }

  getPaymentTypeById(id: number) {
    const url = `${this.baseUrl}/api/PaymentType/${id}`;
    return this.http.get(url);
  }

  createPaymentType(description: string) {
    const url = `${this.baseUrl}/api/PaymentType/${description}`;
    return this.http.post(url, {});
  }

  updatePaymentType(id: number, description: string) {
    const url = `${this.baseUrl}/api/PaymentType/${id}?description=${description}`;
    return this.http.put(url, {});
  }

  deletePaymentType(id: number) {
    const url = `${this.baseUrl}/api/PaymentType/${id}`;
    return this.http.delete(url);
  }

  // Product--------------------------------------------
  // Obtener lista de productos
  getProducts() {
    const url = `${this.baseUrl}/api/Product`;
    return this.http.get(url);
  }

  // Obtener un producto por su barcode
  getProductById(barcode: number) {
    const url = `${this.baseUrl}/api/Product/${barcode}`;
    return this.http.get(url);
  }

  // Obtener un producto por su descripcion
  getProductByDescription(description: string) {
    const url = `${this.baseUrl}/api/Product/${description}`;
    return this.http.get(url);
  }

  // Crear un nuevo producto
  createProduct(
    barcode: number,
    description: string,
    iron: number,
    sodium: number,
    energy: number,
    fat: number,
    calcium: number,
    carbohydrates: number,
    protein: number,
    status: boolean,
    vitamins: string[]
    ){
<<<<<<< Updated upstream
    const encodedArray = vitamins.map(value => encodeURIComponent(value));
    const queryString = encodedArray.join("&");
    const url = `${this.baseUrl}/api/Products?barcode=${barcode}&description=${description}&iron=${iron}&sodium=${sodium}&energy=${energy}&fat=${fat}&calcium=${calcium}&carbohydrates=${carbohydrates}&protein=${protein}&status=${status}&vitamins=`+queryString;
=======
    const url = `${this.baseUrl}/api/Product/${barcode}?description=${description}&iron=${iron}&sodium=${sodium}&energy=${energy}&fat=${fat}&calcium=${calcium}&carbohydrates=${carbohydrates}&protein=${protein}&status=${status}&vitamins=${vitamins}`;
>>>>>>> Stashed changes
    return this.http.post(url, null, {});
  }

  // Actualizar un producto
  updateProduct(
    barcode: number,
    description: string,
    iron: number,
    sodium: number,
    energy: number,
    fat: number,
    calcium: number,
    carbohydrates: number,
    protein: number,
    status: boolean
  ) {
    const url = `${this.baseUrl}/api/Products/${barcode}?description=${description}&iron=${iron}&sodium=${sodium}&energy=${energy}&fat=${fat}&calcium=${calcium}&carbohydrates=${carbohydrates}&protein=${protein}&status=${status}`;
    return this.http.put(url, null, {});
  }

  // Eliminar un producto
  deleteProduct(barcode: number) {
    const url = `${this.baseUrl}/api/Products/${barcode}`;
    return this.http.delete(url);
  }

  // Plan--------------------------------------------

  getPlans() {
    const url = `${this.baseUrl}/api/Plan`;
    return this.http.get(url);
  }

  getPlanById(id: number) {
    const url = `${this.baseUrl}/api/Plan/${id}`;
    return this.http.get(url);
  }

  createPlan(
    nutritionistId: string,
    description: string,
    mealtimeId: number,
    productBarcode: number
  ) {
    const url = `${this.baseUrl}/api/Plan/${description}?nutritionistId=${nutritionistId}&mealtimeId=${mealtimeId}&productBarcode=${productBarcode}`;
    return this.http.post(url, {});
  }

  updatePlan(
    id: number,
    nutritionistId: string,
    description: string,
  ) {
    const url = `${this.baseUrl}/api/Plan/${id}?nutritionistId=${nutritionistId}&description=${description}`;
    return this.http.put(url, {});
  }

  deletePlan(id: number) {
    const url = `${this.baseUrl}/api/Plan/${id}`;
    return this.http.delete(url);
  }

  // Vitamin--------------------------------------------

  getVitamins() {
    const url = `${this.baseUrl}/Vitamins`;
    return this.http.get(url);
  }

  getVitaminById(productBarcode: number) {
    const url = `${this.baseUrl}/Vitamins/${productBarcode}`;
    return this.http.get(url);
  }

  createVitamin(
    productBarcode: number,
    vitamin: string
  ) {
    const url = `${this.baseUrl}/Vitamins?productBarcode=${productBarcode}&vitamin=${vitamin}`;
    return this.http.post(url, {});
  }

  updateVitamin(
    productBarcode: number,
    vitamin: string
  ) {
    const url = `${this.baseUrl}/Vitamins/${productBarcode}?vitamin=${vitamin}`;
    return this.http.put(url, {});
  }

  deleteVitamin(productBarcode: number) {
    const url = `${this.baseUrl}/Vitamins/${productBarcode}`;
    return this.http.delete(url);
  }

  // Mealtime--------------------------------------------

  getMealtimes() {
    const url = `${this.baseUrl}/api/Mealtime`;
    return this.http.get(url);
  }

  getMealtimeById(id: number) {
    const url = `${this.baseUrl}/api/Mealtime/${id}`;
    return this.http.get(url);
  }

  createMealtime(
    id: number,
    name: string,
  ) {
    const url = `${this.baseUrl}/api/Mealtime/${id}?name=${name}`;
    return this.http.post(url, {});
  }

  updateMealtime(
    id: number,
    name: string,
  ) {
    const url = `${this.baseUrl}/api/Mealtime/${id}?name=${name}`;
    return this.http.put(url, {});
  }

  createAdminProductAssociation(adminId: string, productBarcode: number, status: boolean) {
    const url = `${this.baseUrl}/api/AdminProductAssociations/${adminId}/${productBarcode}?status=${status}`;

    return this.http.post(url, null, {});
  }

  createPatientNutrionistAssociation(nutritionistId: string, patientId: string){
    const url = `${this.baseUrl}/api/PatientNutritionistAssociation?nutritionistId=${nutritionistId}&patientId=${patientId}`;
    return this.http.post(url, null, {});
  }

  createPlanPatientAssociation(nutritionistId:string,planId: number, patientId: string, startdate: string, enddate: string){
    const url = `${this.baseUrl}/api/PlanPatientAssociations?nutritionistId=${nutritionistId}&planId=${planId}&patientId=${patientId}&startdate=${startdate}&enddate=${enddate}`;
    return this.http.post(url, null, {});
  }

  createRecipe(description: string, barcodePortion: { [key: number]: number }) {
    const url = `${this.baseUrl}/api/Recipes`;
    const body = { description, barcodePortion };
    return this.http.post(url, body);
  }

}
