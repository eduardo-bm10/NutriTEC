import {Component, Renderer2, ElementRef, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import {GetApiService} from "../get-api.service";

@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.css']
})
export class ClienteComponent implements OnInit{
  constructor(private api:GetApiService){}
  nombre = 'Juan Vainas';
  infoAll = {};
  tipo = 'SPA';
  dropdown = 0;

  pantallaActual = 'principal';

  // ESTO ES UNA COPIA DEL ADMIN COMPONENT, EDITARLO PARA QUE
  //    TENGA LA ESTRUCTURA DE NUTRICIONISTA SEGÚN LA ESPECIFICACIÓN
  // YA CAMBIÉ LOS NOMBRES DE LOS LIST ITEMS :D

  // CAMBIAR NOMBRE DE PANTALLAS PARA NUTRICIONISTA
  pantallas = ["registroMedidas", "registroConsumo", "gestionProductosPlatillos", "gestionRecetas", "reporteAvance"];

  provincias = ["San José", "Alajuela", "Cartago", "Limón", "Guanacaste", "Puntarenas", "Heredia"]


  ngOnInit() {
    for (let i = 0; i < this.pantallas.length; i++) {
      const tmp = document.getElementById(this.pantallas[i]) as HTMLInputElement
      tmp.style.display = 'none'
    }
    //this.cargarProvincias(["gestSucSpaPROVINCIA", "gestEmplPPROVINCIA", "gestEmplPPROVINCIA2"]);
    this.mostrarNombre();
  }
  mostrarNombre(){
    const data = localStorage.getItem('usuario');
    // @ts-ignore
    const info = JSON.parse(data);
    this.infoAll = info;
    this.nombre = `${info['firstname']} ${info['lastname1']} ${info['lastname2']}`;
    const cedula = document.getElementById('cedula') as HTMLInputElement;
    // @ts-ignore
    cedula.value = this.infoAll['id'];
    const nombre = document.getElementById('nombre') as HTMLInputElement;
    // @ts-ignore
    nombre.value = this.infoAll['firstname'];
    const a1 = document.getElementById('apellido1') as HTMLInputElement;
    // @ts-ignore
    a1.value = this.infoAll['lastname1'];
    const a2 = document.getElementById('apellido2') as HTMLInputElement;
    // @ts-ignore
    a2.value = this.infoAll['lastname2'];
    const fecha = document.getElementById('fechaDeNacimiento') as HTMLInputElement;
    // @ts-ignore
    fecha.value = this.infoAll['birthday'];
  }

  //Función utilizada para cargar cada una de las 7 provincias en los componentes select que las necesitan
  cargarProvincias(lista:any){
    for (let i = 0; i < lista.length; i++) {
      const p = document.getElementById(lista[i]) as HTMLSelectElement;
      for (let i = 0; i < this.provincias.length; i++) {
        const provincia = this.provincias[i];
        const option = new Option(provincia, provincia);
        p.add(option);
      }
    }
  }

  //Utilizado para los modificar la vista actual y mostrar solamente los componentes que son necesarios
  mostrar(idPrincipal:string, idDrop:string){
    const principal = document.getElementById(idPrincipal) as HTMLInputElement
    const drop = document.getElementById(idDrop) as HTMLInputElement

    if(principal.getAttribute('aria-expanded') === 'true'){
      principal.className = 'nav-link collapsed';
      principal.setAttribute('aria-expanded', 'false')
      drop.className = 'collapse'
    }
    else{ //se debe desplegar el dropdown
      principal.className = 'nav-link';
      principal.setAttribute('aria-expanded', 'true')
      drop.className = 'collapse show'
    }
  }

  //Cicla entre cada una de las ventanas posibles y muestra la que se solicita
  mostrarPantalla(pantalla:string){
    const act = document.getElementById(this.pantallaActual) as HTMLInputElement
    act.style.display = 'none'

    const tmp = document.getElementById(pantalla) as HTMLInputElement
    tmp.style.display = 'block'
    console.log(tmp)

    this.pantallaActual = tmp.id
  }

  //Muestra los componentes desplegables que pertenecen a ciertas opciones de mostrar ventanas
  mostrarDropdown(){
    const drop = document.getElementById("dropdown") as HTMLInputElement
    if(this.dropdown === 0){
      drop.className = "dropdown-menu dropdown-menu-right shadow animated--grow-in show"
      this.dropdown = 1
    }
    else{
      drop.className = "dropdown-menu dropdown-menu-right shadow animated--grow-in"
      this.dropdown = 0
    }
  }

  toNum(dato:string):number{
    return parseInt(dato, 10);
  }

  logout(){
    this.api.logout();
  }
}
