import {Component, OnInit} from '@angular/core';
import { GetApiService } from '../get-api.service';
import { HtmlParser } from '@angular/compiler';
import { SharedService} from "../shared.service";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit{
  constructor(private api:GetApiService, private sharedService: SharedService){}
  nombre = 'TEC admin';
  tipo = 'SPA';
  dropdown = 0;

  pantallaActual = 'principal';

  pantallas = ["aprobacionProductos","reporteCobro"];

  provincias = ["San José", "Alajuela", "Cartago", "Limón", "Guanacaste", "Puntarenas", "Heredia"]

  opcionesGlobales = {
    "productosFalsos" : {},
    "productosTotales" : {},
    "cobros" : {}
  }

  titulosTabla:string[] = [];
  contenido:string[][] = [];

  ngOnInit() {
    for (let i = 0; i < this.pantallas.length; i++) {
      const tmp = document.getElementById(this.pantallas[i]) as HTMLInputElement
      tmp.style.display = 'none'
    }
    this.cargarProductos();
    this.cargarCobro();
    this.cargarProductosTotales();
    this.cargarProvincias(["gestSucSpaPROVINCIA", "gestEmplPPROVINCIA", "gestEmplPPROVINCIA2"]);
  }

  cambiarInfo(llegada:string, seleccionador:string, id:string, des:string){
    const tmp = document.getElementById(seleccionador) as HTMLInputElement;
    const idA =  document.getElementById(id) as HTMLInputElement;
    const desA =  document.getElementById(des) as HTMLInputElement;
    //@ts-ignore
    const info = this.opcionesGlobales[llegada];
    for(const op in info){
      if(tmp.value == info[op].descripcion){
        idA.value = info[op].identificador;
        desA.value = info[op].descripcion;
      }
    }
  }

  cargarProductos(){
    this.api.getProductByStatus(false).subscribe((data) => {
      const llegada = JSON.parse(JSON.stringify(data));
      //console.log(llegada);
      this.opcionesGlobales.productosFalsos = llegada;
      const tmp = document.getElementById("aprobacionProductosSelect") as HTMLInputElement;

      for(const op in llegada){
        const aux = llegada[op];
        const opcionTmp = document.createElement('option');
        opcionTmp.value = aux.barcode;
        opcionTmp.textContent = aux.barcode;
        tmp.appendChild(opcionTmp);
      }

      this.cambiarInfo('productosFalsos', 'aprobacionProductosSelect', 'TEST', 'TEST');
    })
  }

  cargarCobro(){
    this.api.getPaymentTypes().subscribe((data) => {
      const llegada = JSON.parse(JSON.stringify(data));
      console.log(llegada);
      this.opcionesGlobales.cobros = llegada;
      const tmp = document.getElementById("tipoDeCobroSelect") as HTMLInputElement;

      for(const op in llegada){
        const aux = llegada[op];
        const opcionTmp = document.createElement('option');
        opcionTmp.value = aux.id;
        opcionTmp.textContent = aux.id;
        tmp.appendChild(opcionTmp);
      }

      this.cambiarInfo('cobros', 'tipoDeCobroSelect', 'TEST', 'TEST');
    })
  }

  cargarProductosTotales(){
    this.api.getProducts().subscribe(data => {
      const llegada = JSON.parse(JSON.stringify(data));
      console.log(llegada);
      this.opcionesGlobales.productosTotales = llegada;
      const tmp = document.getElementById("aprobacionProductosTotalSelect") as HTMLInputElement;
      const textarea = document.getElementById("aprobacionProductosDatos") as HTMLTextAreaElement;

      for(const op in llegada){
        const aux = llegada[op];
        const opcionTmp = document.createElement('option');
        opcionTmp.value = aux.barcode;
        opcionTmp.textContent = aux.barcode;
        tmp.appendChild(opcionTmp);

        const info = llegada[op];
        textarea.append(`Codigo: ${llegada[op].barcode}. Nombre: ${llegada[op].description}, Hierro: ${llegada[op].iron}, Sodio: ${llegada[op].sodium}, Energia: ${llegada[op].energy} , Grasa: ${llegada[op].fat}, Calcio: ${llegada[op].calcium}, Carbohidratos: ${llegada[op].carbohydrate}, Proteina: ${llegada[op].protein}\n`)
      }




      this.cambiarInfo('productosTotales', 'aprobacionProductosTotalSelect', 'TEST', 'TEST');
    })
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


  aprobarProducto(){
    const barras = document.getElementById('aprobacionProductosSelect') as HTMLInputElement;
    const barra = parseInt(barras.value)
    var usuario = localStorage.getItem("usuario");

    console.log(barra)

    if (usuario !== null) {
      var user = JSON.parse(usuario);
      /*this.api.updateProductStatus(user.id,barra,true).subscribe(data => {
        console.log(data);
      });*/
      this.api.createAdminProductAssociation(user.id, barra, true).subscribe(data => {
        console.log(data)
      })
    }
  }


  eliminarProducto(){
    const barras = document.getElementById('aprobacionProductosTotalSelect') as HTMLInputElement;
    if(confirm("Are you sure to delete "+ barras.value)) {
      this.api.deleteProduct(Number(barras.value)).subscribe((data) => {
        console.log(data)
      });
    }
  }

  generarReporte(){
    const pago = document.getElementById('tipoDeCobroSelect') as HTMLInputElement;
    this.api.paymentReport(Number(pago.value)).subscribe(data => {
      const llegada = JSON.parse(JSON.stringify(data));
      const area = document.getElementById("reporteCobroArea") as HTMLTextAreaElement;
      area.value = "";
      let aux:string = "";

      this.titulosTabla = ['EMAIL', 'NOMBRE', 'PAGO', 'DESCUENTO', 'PAGOF'];

      for (let key in llegada) {
        const listaAux = [llegada[key].email.toString(), llegada[key].fullName.toString(),
          llegada[key].totalPayment.toString(), llegada[key].discount.toString(), llegada[key].finalPayment.toString()];

        this.contenido.push(listaAux);

        aux+=("Email: " + llegada[key].email.toString() + ", ");
        aux+=("Nombre completo: " + llegada[key].fullName.toString() + ", ");
        aux+=("Pago total: " + llegada[key].totalPayment.toString() + ", ");
        aux+=("Descuento: " + llegada[key].discount.toString() + ", ");
        aux+=("Pago final: " + llegada[key].finalPayment.toString() + ".");
        aux+=('\n\n');
      }

      area.value = aux.toString();

    })
  }

  imprimirReporte(){
    const info = {
      'titulos' : this.titulosTabla,
      'data': this.contenido,
      'ruta': '/admin'
    }
    this.sharedService.jsonData = info;
    this.api.ruta("/reporte");
  }

  logOut(){
    this.api.logout();
  }

}//bracket que cierras


