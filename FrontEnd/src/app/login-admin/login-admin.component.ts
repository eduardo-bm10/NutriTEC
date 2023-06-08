import {Component, OnInit} from '@angular/core';
import { GetApiService } from '../get-api.service';

@Component({
  selector: 'app-login-admin',
  templateUrl: './login-admin.component.html',
  styleUrls: ['./login-admin.component.css']
})
export class LoginADMINComponent implements OnInit{
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
    console.log(valor);
    this.api.login(
      valor.email, valor.password
    ).subscribe(data => {
      const llegada = JSON.parse(JSON.stringify(data));
      console.log(llegada);
      if(llegada[1] == ''){
        localStorage.setItem('usuario', JSON.stringify(llegada[0]));
        this.api.ruta('/paciente');
      }
      else if (llegada.tipo == 'administrator') {
        localStorage.setItem('usuario', JSON.stringify(llegada.usuario));
        console.log(llegada.usuario);
        this.api.ruta('/admin');
      }
      else{
        alert('Error!');
      }
    })
  }

  register(form:any){
    const valor = form.value;
  }
}
