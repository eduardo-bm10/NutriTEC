import {Component, OnInit} from '@angular/core';
import { GetApiService } from '../get-api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  constructor(private api:GetApiService){}
  mostrar = false

  ngOnInit() {
    const signin = document.getElementById("signin") as HTMLInputElement
    signin.style.display = 'none'
    this.getPaises();
  }

  mostrarContra(){
    const input = document.getElementById('floatingPassword') as HTMLInputElement
    if(this.mostrar){
      input.type = 'password'
    }
    else{
      input.type = 'text'
    }
    this.mostrar = !this.mostrar
  }

  mostrarContra2(){
    const input = document.getElementById('floatingPassword1') as HTMLInputElement
    if(this.mostrar){
      input.type = 'password'
    }
    else{
      input.type = 'text'
    }
    this.mostrar = !this.mostrar
  }

  cambiarPantalla(type:number, event:Event){
    event.preventDefault();
    const signin = document.getElementById("signin") as HTMLInputElement
    const login = document.getElementById("login") as HTMLInputElement

    if(type === 0){
      login.style.display = 'none'
      signin.style.display = 'block'
    }
    else{
      signin.style.display = 'none'
      login.style.display = 'block'

    }
  }
  login(form:any){
    const valor = form.value;
    this.api.login(
      valor.email, valor.password
    ).subscribe(data => {
      const llegada = JSON.parse(JSON.stringify(data));
      if(llegada[1] == 'patient'){
        localStorage.setItem('usuario', JSON.stringify(llegada[0]));
        this.api.ruta('/paciente');
      }
      else{
        alert('Error!');
      }
    })
  }

  register(form:any){
    const valor = form.value;
    this.api.createPatient(
      valor.cedula, valor.nombre, valor.apellido1, valor.apellido2, valor.email,
      valor.password, Number(valor.peso), Number(valor.imc), valor.direccion, valor.fechaDeNacimiento,
      valor.pais, Number(valor.calorias), Number(valor.cintura), Number(valor.cuello), Number(valor.caderas),
        Number(valor.musculo), Number(valor.grasa)
    ).subscribe((data) => {
      const llegada = JSON.parse(JSON.stringify(data));
    })
  }

  createOption(texto:string){
    const option = document.createElement("option");
    option.text = texto;
    option.value = texto;
    return option;
  }
  getPaises(){
    fetch('https://restcountries.com/v3.1/all')
      .then(response => response.json())
      .then(data => {
        const select = document.getElementById("pais") as HTMLSelectElement;
        const todos:string[] = [];
        data.forEach((info: any) => {
          todos.push(info.name.common)
        })

        todos.sort();
        todos.forEach((pais: any) => {
          select.add(this.createOption(pais))
        })
      })
      .catch(error => {
        alert(`Error al realizar la solicitud ---- ${error.message}:`);
      });
  }

  logout(){
    this.api.logout();
  }
}
