import {Component, Renderer2, ElementRef, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.css']
})
export class ClienteComponent implements OnInit{
  constructor(private renderer: Renderer2, private el: ElementRef,private router:Router,private route: ActivatedRoute ) { }

  dropdown = 0

  nombre = "Cristiano Ronaldo"
  activo = 'principal'

  pantallas = ["registroMedidas", "registroConsumo", "gestionProductosPlatillos", "gestionRecetas", "reporteAvance"];
  ngOnInit() {
    for (let i = 0; i < this.pantallas.length; i++) {
      const tmp = document.getElementById(this.pantallas[i]) as HTMLInputElement
      tmp.style.display = 'none'
    }
  }

  extraerCedula():string{
    var cedula = ""
    this.route.params.subscribe(params => {
      cedula = params['cedula'];
    })
    return cedula;
  }


  desbloquear(vista:string){
    const activo = document.getElementById(this.activo) as HTMLInputElement
    activo.style.display = 'none'
    //buscamos el nuevo
    const nuevo = document.getElementById(vista) as HTMLInputElement
    nuevo.style.display = 'block'
    this.activo = vista
  }

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

  cambiarPantalla(pantalla:string){
    this.desbloquear(pantalla)
  }

  clase(tipo:string){
    this.cambiarPantalla('registro')
    const titulo = document.getElementById('registroName') as HTMLInputElement
    titulo.innerHTML = `Registro de ${tipo}`

  }

  crearCartaClase(tipo:string, src:string, caption:string){
    const carta = document.createElement('figure')
    carta.className = 'cartaClase'
    carta.addEventListener("click", () => {
      this.clase(tipo);
    });
    const imagen = document.createElement('img');
    imagen.src = src
    carta.appendChild(imagen)

    const figcap = document.createElement('figcaption')
    figcap.innerHTML = caption
    carta.appendChild(figcap)

    return carta
  }

  buscarClases(){
    const inicio = document.getElementById('fechaInicio') as HTMLInputElement
    const final = document.getElementById('fechaFinaliza') as HTMLInputElement
    const sedes = document.getElementById('sede') as HTMLInputElement

    if(inicio && final && sedes){
      const claseCartas = document.getElementById('clasesCartas') as HTMLInputElement
      claseCartas.innerHTML = ''
      const fi = inicio.value
      const ff = final.value
      const sede = sedes.value
      if(fi != "" && ff != ""){
        const div = document.getElementById('clasesCartas') as HTMLInputElement
        div.appendChild(this.crearCartaClase('natacion', 'https://img.freepik.com/premium-vector/continuous-line-drawing-young-professional-swimmer-practicing-swimming-indoor-pool-sports_500861-500.jpg', 'Natacion'))
        div.appendChild(this.crearCartaClase('pilates', 'https://t3.ftcdn.net/jpg/03/72/43/12/360_F_372431283_rhVo7VWm2Fmgf2K0CBo99cbZuvfoCdau.jpg', 'Pilates'))
      }
      else{
        alert("Los datos de Fecha de Inicio, Fecha de Finalización y Sede no deben estar vacíos!!")
      }
    }
  }
  logout(){
    alert("logout")
  }
}
