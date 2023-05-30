import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GetApiService {

  private baseUrl = 'https://postgresqlapi.azurewebsites.net';

  constructor(private http: HttpClient) {} 
  
  // Iniciar sesión
  login(email: string, password: string) {
    const url = `${this.baseUrl}/api/Login/login?email=${email}&password=${password}`;
    return this.http.post(url, null);
  }
  // Administrador
  
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

  // Nutritionist
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
    address: string,
    paymentid: number,
    photo: string
  ) {
    const url = `${this.baseUrl}/api/Nutritionists?id=${id}&nutritionistcode=${nutritionistcode}&firstname=${firstname}&lastname1=${lastname1}&lastname2=${lastname2}&email=${email}&password=${password}&weight=${weight}&bmi=${bmi}&address=${address}&paymentid=${paymentid}&photo=${photo}`;

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

  // Patient
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
}
