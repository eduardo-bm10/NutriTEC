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
    this.getIdTipoCobro();
  }

  getIdTipoCobro(){
    this.api.getPaymentTypes().subscribe(data =>{
      const llegada = JSON.parse(JSON.stringify(data));
      const select = document.getElementById('tipoCobro') as HTMLSelectElement;
      for (const key in llegada) {
        if (llegada.hasOwnProperty(key)) {
          const todo = llegada[key];
          const id = todo['id'];
          const nombre = todo['description'];

          const opcionTmp = document.createElement('option');
          opcionTmp.value = id;
          opcionTmp.textContent = nombre;
          select.appendChild(opcionTmp);
        }
      }
      console.log(select.value);
    })
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


  convertirRutaAFile(rutaImagen: string): File {
    return new File([rutaImagen], "nombreArchivo.jpg", { type: "image/jpeg" });
  }

  llamadaApiRegistrar(valor:any, foto:string){
    this.api.createNutritionist(
      valor.cedula, valor.codigoNutri, valor.nombre, valor.apellido1, valor.apellido2,
      valor.email, valor.password, valor.peso, valor.imc, valor.tarjeta, valor.direccion,
      valor.tipoCobro, foto
    ).subscribe(data => {
      const llegada = JSON.parse(JSON.stringify(data));
      console.log(llegada);
    })
  }


  register(form:any){
    const valor = form.value;

    const file = this.convertirRutaAFile(valor.foto);
    const reader = new FileReader();
    reader.onloadend = () => {
      const result = reader.result as string;
      console.log(valor.tipoCobro)
      //this.llamadaApiRegistrar(valor, result.split(',')[1]);
    };
    reader.readAsDataURL(file);
  }
}
