import {Component, OnInit} from '@angular/core';
import {GetApiService} from '../get-api.service';

@Component({
  selector: 'app-nutri',
  templateUrl: './nutri.component.html',
  styleUrls: ['./nutri.component.css']
})

export class NutriComponent implements OnInit {
  nombre = 'Nutricionista Lic. María Fernanda Sánchez';
  totalCalorias = 0;
  dropdown = 0;

  pantallaActual = 'principal';

  // ESTO ES UNA COPIA DEL ADMIN COMPONENT, EDITARLO PARA QUE
  //    TENGA LA ESTRUCTURA DE NUTRICIONISTA SEGÚN LA ESPECIFICACIÓN
  // YA CAMBIÉ LOS NOMBRES DE LOS LIST ITEMS :D

  // CAMBIAR NOMBRE DE PANTALLAS PARA NUTRICIONISTA
  pantallas = ["registro", "gestionProductos", "busquedaAsociacionClientesComoPacientes", "gestionPlanes", "asignacionPlan", "seguimientoPaciente"];

  provincias = ["San José", "Alajuela", "Cartago", "Limón", "Guanacaste", "Puntarenas", "Heredia"]


  ngOnInit() {
    for (let i = 0; i < this.pantallas.length; i++) {
      const tmp = document.getElementById(this.pantallas[i]) as HTMLInputElement
      tmp.style.display = 'none'
    }
    //this.cargarProvincias(["gestSucSpaPROVINCIA", "gestEmplPPROVINCIA", "gestEmplPPROVINCIA2"]);
    this.cargarCheckBoxesComidas();
    const calorias = document.getElementById('gestionPlanesCaloriasExistente') as HTMLInputElement;
    calorias.value = String(this.totalCalorias);
  }

  //Función utilizada para cargar cada una de las 7 provincias en los componentes select que las necesitan
  cargarProvincias(lista: any) {
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
  mostrar(idPrincipal: string, idDrop: string) {
    const principal = document.getElementById(idPrincipal) as HTMLInputElement
    const drop = document.getElementById(idDrop) as HTMLInputElement

    if (principal.getAttribute('aria-expanded') === 'true') {
      principal.className = 'nav-link collapsed';
      principal.setAttribute('aria-expanded', 'false')
      drop.className = 'collapse'
    } else { //se debe desplegar el dropdown
      principal.className = 'nav-link';
      principal.setAttribute('aria-expanded', 'true')
      drop.className = 'collapse show'
    }
  }

  //Cicla entre cada una de las ventanas posibles y muestra la que se solicita
  mostrarPantalla(pantalla: string) {
    const act = document.getElementById(this.pantallaActual) as HTMLInputElement
    act.style.display = 'none'

    const tmp = document.getElementById(pantalla) as HTMLInputElement
    tmp.style.display = 'block'
    console.log(tmp)

    this.pantallaActual = tmp.id
  }

  //Muestra los componentes desplegables que pertenecen a ciertas opciones de mostrar ventanas
  mostrarDropdown() {
    const drop = document.getElementById("dropdown") as HTMLInputElement
    if (this.dropdown === 0) {
      drop.className = "dropdown-menu dropdown-menu-right shadow animated--grow-in show"
      this.dropdown = 1
    } else {
      drop.className = "dropdown-menu dropdown-menu-right shadow animated--grow-in"
      this.dropdown = 0
    }
  }

  esImpar(numero: number): boolean {
    return numero % 2 === 1;
  }

  sumarCalorias(id:string){
    const elemento = document.getElementById(id) as HTMLInputElement;
    if(elemento.checked){
      // @ts-ignore
      this.totalCalorias += parseInt(elemento.getAttribute('calorias'));
    }
    else{
      // @ts-ignore
      this.totalCalorias -= parseInt(elemento.getAttribute('calorias'));
    }
    const calorias = document.getElementById('gestionPlanesCaloriasExistente') as HTMLInputElement;
    calorias.value = String(this.totalCalorias);
  }

  checkBox(nombre:string, calorias:string, id:string){
    const div = document.createElement('div');
    const label = document.createElement('label');
    const input = document.createElement('input');


    label.innerText = nombre;
    input.className = "form-control form-control-user";
    input.type = 'checkbox';
    input.setAttribute('calorias', calorias);
    input.id = id + "-comida";
    input.addEventListener('change', () => {
      this.sumarCalorias(id + "-comida");
    });

    div.appendChild(label);
    div.appendChild(input);
    return div;
  }

  cargarCheckBoxesComidas(){
    let todo = {
      "pollo": {
        "calorias": "500",
        "grasa": "80",
        "proteinas": "30",
        "vararandom": "60",
        "test": "170",
        "hpta": "69"
      },

      "arroz": {
        "calorias": "50",
        "grasa": "80",
        "proteinas": "30",
        "vararandom": "60",
        "test": "170",
        "hpta": "67"
      },

      "frijoles": {
        "calorias": "10",
        "grasa": "80",
        "proteinas": "30",
        "vararandom": "60",
        "test": "170",
        "hpta": "7869"
      },

      "lentejas": {
        "calorias": "545",
        "grasa": "80",
        "proteinas": "30",
        "vararandom": "60",
        "test": "170",
        "hpta": "234"
      }
    }
    const length = Object.keys(todo).length;
    let mitad = 0;
    if(this.esImpar(length)){
      mitad = length/2+1;
    }
    else{
      mitad = length/2;
    }

    let aux = 0;
    const comidas1 = document.getElementById('comidas1') as HTMLDivElement;
    const comidas2 = document.getElementById('comidas2') as HTMLDivElement;
    for (const key in todo) {
      // @ts-ignore
      const data = todo[key];
      const check = this.checkBox(key, data['calorias'], data['hpta']);
      if(aux < mitad){
        comidas1.appendChild(check);
      }
      else{
        comidas2.appendChild(check);
      }
      aux ++;
    }

  }
}
