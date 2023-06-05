import {Component, OnInit} from '@angular/core';
import { GetApiService } from '../get-api.service';

@Component({
  selector: 'app-login-nutri',
  templateUrl: './login-nutri.component.html',
  styleUrls: ['./login-nutri.component.css']
})
export class LoginNUTRIComponent implements OnInit{
  constructor(private api:GetApiService){}
  mostrar = false

  ngOnInit() {
    const signin = document.getElementById("signin") as HTMLInputElement
    signin.style.display = 'none'

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
      console.log("a")
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
      console.log(llegada);
      if(llegada[1] == ''){
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
    this.api.createNutritionist(
      valor.cedula, valor.codigoNutri, valor.nombre, valor.apellido1, valor.apellido2,
      valor.email, valor.password, valor.peso, valor.imc, valor.tarjeta, valor.direccion, 
      valor.tipoCobro, valor.foto
    ).subscribe(data => {
      const llegada = JSON.parse(JSON.stringify(data));
      console.log(llegada);
    })
  }
}
