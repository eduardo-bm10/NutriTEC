import {Component, OnInit} from '@angular/core';
import { GetApiService } from '../get-api.service';
import { HtmlParser } from '@angular/compiler';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit{
  constructor(private api:GetApiService){}
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

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (agrega una sucursal a la db)
activarSucursal(){
  const sede = document.getElementById('sedegestSucSpaSELECT') as HTMLInputElement;
  const nombre = document.getElementById('gestSucSpaNOMBRE') as HTMLInputElement;
  const fechaDeApertura = document.getElementById('gestSucSpaFECHAAPERTURA') as HTMLInputElement;
  const empleadoAdmin = document.getElementById('gestSucSpaEMPLEADOADMINISTRADOR') as HTMLInputElement;
  const capacidad = document.getElementById('gestSucSpaCAPACIDAD') as HTMLInputElement;
  const numerosTelefono = document.getElementById('gestSucTiendaNUMEROS2') as HTMLInputElement;
  const provincia = document.getElementById("gestSucSpaPROVINCIA") as HTMLInputElement;
  const canton = document.getElementById("gestSucSpaCANTON") as HTMLInputElement;
  const distrito = document.getElementById("gestSucSpaDISTRITO") as HTMLInputElement;
  const horaApertura = document.getElementById('gestSucSpaHORARIOAPERTURA') as HTMLInputElement;
  const horaCierre = document.getElementById('gestSucSpaHORARIOCIERRE') as HTMLInputElement;

}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (activa el spa correspondiente a una sede)
activacionSpa(){ //FUNCIONA, ACATAR DETALLE
  const sede = document.getElementById('gestSucTiendaNUMEROSSEDESPA') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (activa la tienda correspondiente a una sede)
activacionTienda(){//FUNCIONA, ACATAR DETALLE
  const sede = document.getElementById('gestSucTiendaNUMEROSSEDETIENDA') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (Modifica un tratamiento ya existente)
agregarTratamiento(){ //ESTE ES EL DE MODIFICAR, NO ESTA
  const tratamiento = document.getElementById('gestTratSpaPSELECT') as HTMLInputElement;
  const IDunico = document.getElementById('gestTratSpaID') as HTMLInputElement;
  const nombre = document.getElementById('gestTratSpaNOMNRE') as HTMLInputElement;

  //STANDBY
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (elimina un tratamineto ya existente)
eliminarTratamiento(){
  const tratamiento = document.getElementById('gestTratSpaPSELECT') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (agrega un tratamiento nuevo a la db)
agregarNuevoTratamiento(){
  const nombreNuevo = document.getElementById('gestTratSpaNOOMBRENUEVO') as HTMLInputElement; //TENER CUIDADO, VER SI HAY QUE ELIMINAR LA CASILLA DE NOMBRE Y TRABAJAR CON DESCRIPCION
}


//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (modifica un puesto ya existente)
guardarPuesto(){ //No hay para modificar este valo
  const puesto = document.getElementById('gestPuestPSELECT') as HTMLInputElement;
  const ID = document.getElementById('gestPuestPID') as HTMLInputElement;
  const descripcion = document.getElementById('gestPuestPDESCRIPCION') as HTMLInputElement;
  //STANDBY
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (elimina un puesto ya existente)
eliminarPuesto(){
  const descripcion = document.getElementById('gestPuestPSELECT') as HTMLInputElement; //VERIFICAR ESTE, SACAR ID DEL PUESTO Y PASARA ESE PARAMETRO
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (agrega un nuevo puesto a la db)
agregarNuevoPuesto(){ //ELIMINAR CASILLA DE NOMBRE DEL HTML
  const descripcion = document.getElementById('gestPuestPDESCRIPCIONAGREGAR') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados ()
gestionDePlanilla(){
  const tipoPlanilla = document.getElementById('gestTipPlaPSELECT') as HTMLInputElement;
  const id = document.getElementById('gestTipPlaPID') as HTMLInputElement;
  const pagoMensual = document.getElementById('gestTipPlaPPAGOMENSUAL') as HTMLInputElement;
  const pagoHoras = document.getElementById('gestTipPlaPPAGOPORHORAS') as HTMLInputElement;
  const pagoClase = document.getElementById('gestTipPlaPPAGOPORCLASE') as HTMLInputElement;
  const descripcion = document.getElementById('gestTipPlaPDESCRIPCION') as HTMLInputElement;

  //aqui si no comprendo que llamada hacer
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (modifica los datos de un empleado ya existente)
gestionEmpleados(){ //FUNCA
  const empleado = document.getElementById('gestEmplPSELECT') as HTMLInputElement;
  const cedula = document.getElementById('gestEmplPNUMEROCEDULA') as HTMLInputElement;
  const nombre = document.getElementById('gestEmplPNOMBRE') as HTMLInputElement;
  const puestoQueDesem = document.getElementById('gestEmplPPUESTO') as HTMLInputElement;
  const sucursalQueTrabaja = document.getElementById('gestEmplPSUCURSAL') as HTMLInputElement;
  const tipoDePlanilla = document.getElementById('gestEmplPLANILLA') as HTMLInputElement;
  const salario = document.getElementById('gestEmplPSALARIO') as HTMLInputElement;
  const correo = document.getElementById('gestEmplPCORREO') as HTMLInputElement;
  const contrasena = document.getElementById('gestEmplPPASSWORD') as HTMLInputElement;
  const apellido1 = document.getElementById('gestEmplPPRIMERAPELLIDO') as HTMLInputElement;
  const apellido2 = document.getElementById('gestEmplPSEGUNDOAPELLIDO') as HTMLInputElement;
  const distrito = document.getElementById('gestEmplPDISTRITO') as HTMLInputElement;
  const canton = document.getElementById('gestEmplPCANTON') as HTMLInputElement;
  const provincia = document.getElementById('gestEmplPPROVINCIA') as HTMLInputElement;
  //modificarEmpleado
  //@ts-ignore

  //AQUI STANDBY, NO HAY MODIFICAR
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (elimina un empleado ya existente)
eliminarEmpleado(){ //FUNCA
  const cedula = document.getElementById('gestEmplPNUMEROCEDULA') as HTMLInputElement;
  //@ts-ignore
  this.auth.eliminarEmpleados(this.toNum(cedula.value));
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (agrega un empleado nuevo a la db)
agregarEmpleado(){ //FUNCA
  const cedula = document.getElementById('gestEmplPNUMEROCEDULA2') as HTMLInputElement;
  const nombre = document.getElementById('gestEmplPNOMBRE2') as HTMLInputElement;
  const puestoQueDesem = document.getElementById('gestEmplPPUESTO2') as HTMLInputElement;
  const sucursalQueTrabaja = document.getElementById('gestEmplPSUCURSAL2') as HTMLInputElement;
  const tipoDePlanilla = document.getElementById('gestEmplPLANILLA2') as HTMLInputElement;
  const salario = document.getElementById('gestEmplPSALARIO2') as HTMLInputElement;
  const correo = document.getElementById('gestEmplPCORREO2') as HTMLInputElement;
  const contrasena = document.getElementById('gestEmplPPASSWORD2') as HTMLInputElement;
  const apellido1 = document.getElementById('gestEmplPPRIMERAPELLIDO2') as HTMLInputElement;
  const apellido2 = document.getElementById('gestEmplPSEGUNDOAPELLIDO2') as HTMLInputElement;
  const distrito = document.getElementById('gestEmplPDISTRITO2') as HTMLInputElement;
  const canton = document.getElementById('gestEmplPCANTON2') as HTMLInputElement;
  const provincia = document.getElementById('gestEmplPPROVINCIA2') as HTMLInputElement;
  //@ts-ignore
  //LA FUNCION LOS TRABAJA COMO IDs LOS PARAMETRO DE SUCURSAL, PUESTO Y PLANTILLA, HAY QUE OBTENERLOS COMO TAL
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (modifica un servicio ya existente)

guardarServicio(){
  const servicio = document.getElementById('gestServPSELECT') as HTMLInputElement;
  const IDunico = document.getElementById('gestServPID') as HTMLInputElement;
  const nombre = document.getElementById('gestServPNOMBRE') as HTMLInputElement;
  const pagoHoras = document.getElementById('gestServPDESCRIPCION') as HTMLInputElement;

  //STANDBY
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (elimina un servicio ya existente)
eliminarServicio(){
  const servicio = document.getElementById('gestServPSELECT') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (agrega un nuevo servicio a la db)
agregarNuevoServicio(){
  const descripcion = document.getElementById('gestServPDESCRIPCION2') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (modifica un tipo de equipo ya existente)
guardarTipoEquipo(){ //FUNCA, VERIFICAR QUE SEA SI CON LA DESCRIPCION QUE SE QUIERE TRABAJAR
  //const gym = document.getElementById('gestTipEquipPGYM') as HTMLInputElement;
  // id = document.getElementById('gestTipEquipPID') as HTMLInputElement;
  const descripcion = document.getElementById('gestTipEquipPDESCRIPCION') as HTMLInputElement;

  //no hay metodo de modificacion
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (elimina un tipo de equipo ya existente)
eliminarTipoEquipo(){
  const descripcion = document.getElementById('gestTipEquipPDESCRIPCION') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (agrega un nuevo de equipo a la db)
agregarNuevoEquipo(){
 // const id = document.getElementById('gestTipEquipPIDNUEVO') as HTMLInputElement;
  const descripcion = document.getElementById('gestTipEquipPDESCRIPCIONNUEVO') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (modifica un inventario ya existente)
guardarInventario(){
  const gym = document.getElementById('gestInvetPGYM') as HTMLInputElement;
  const maquina = document.getElementById('gestInvetPMAQUINA') as HTMLInputElement;
  const tipoDeEquipo = document.getElementById('gestInvetPTIPO') as HTMLInputElement;
  const marca = document.getElementById('gestInvetPMARCA') as HTMLInputElement;
  const numeroDeSerie = document.getElementById('gestInvetPNUMEROSERIE') as HTMLInputElement;
  const costo = document.getElementById('gestInvetPCOSTOSUCURSAL') as HTMLInputElement;
  const asignadaAGym = document.getElementById('gestInvetPASIGNADA') as HTMLInputElement;

  //STANDBY
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (elimina un inventario ya existente)
eliminarInventario(){ //FUNCA, SI NO LO HACE ES PORQUE TRABAJA CON IDS
  const numeroDeSerie = document.getElementById('gestInvetPNUMEROSERIE') as HTMLInputElement;
}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (agrega un nuevo inventario a la db)
agregarNuevoInventario(){
  const tipoDeEquipo = document.getElementById('gestInvetPTIPO2') as HTMLInputElement;
  const marca = document.getElementById('gestInvetPMARCA2') as HTMLInputElement;
  const numeroDeSerie = document.getElementById('gestInvetPNUMEROSERIE2') as HTMLInputElement;

}

//Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (modifica un producto ya creado)
  modificarProducto(){
    const gym = document.getElementById('gestProductPGYM') as HTMLInputElement;
    const producto = document.getElementById('gestProductPPRODUCTO') as HTMLInputElement;
    const nombre = document.getElementById('gestProductPNOMBRE') as HTMLInputElement;
    const numeroBarras = document.getElementById('gestProductPCODIGO') as HTMLInputElement;
    const descripcion = document.getElementById('gestProductPDESCRIPCION') as HTMLInputElement;
    const costo = document.getElementById('gestProductPCOSTO') as HTMLInputElement;
    //STANDBY
  }



  //Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (agrega un nuevo producto a la db)
  agregarProducto(){
    const gym = document.getElementById('gestProductPGYMNUEVO') as HTMLInputElement;
    const nombre = document.getElementById('gestProductPNOMBRENUEVO') as HTMLInputElement;
    const numeroBarras = document.getElementById('gestProductPCODIGONUEVO') as HTMLInputElement;
    const descripcion = document.getElementById('gestProductPDESCRIPCIONNUEVO') as HTMLInputElement;
    const costo = document.getElementById('gestProductPCOSTONUEVO') as HTMLInputElement;
  }

 //Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (asocia un tratamiento a un spa)
  asociarTratamientoASpa(){
    const spa = document.getElementById('confGymPSpaSPA') as HTMLInputElement;
    const tratamiento = document.getElementById('confGymPSpaTratamiento') as HTMLInputElement;
  }

  //Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (asocia un producto a una tienda)
  asociarProductosATienda(){
    const gym = document.getElementById('confGymPProducSELECT') as HTMLInputElement;
    const producto = document.getElementById('confGymPProducASOCIAR') as HTMLInputElement;
  }

  //Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (asocia un inventario a un gym y le da un costo)
  asociarInventario(){
    const gym = document.getElementById('confGymPInventarioGYM') as HTMLInputElement;
    const equipo = document.getElementById('confGymPInventarioEQUIPO') as HTMLInputElement;
    const costo = document.getElementById('confGymPInventarioCOSTO') as HTMLInputElement;
  }

  //Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (crea una clase y la agrega a la db)
  crearClase(){
    const clase = document.getElementById('confGymPCrearSELECT') as HTMLInputElement;
    const tipo = document.getElementById('confGymPCrearTIPO') as HTMLInputElement;
    const instructor = document.getElementById('confGymPCrearINSTRUCTOR') as HTMLInputElement;
    const grupalOno = document.getElementById('confGymPCrearGRUPOIND') as HTMLInputElement;
    const capacidad = document.getElementById('confGymPCrearCAPACIDAD') as HTMLInputElement;
    const fecha = document.getElementById('confGymPCrearFECHA') as HTMLInputElement;
    const horaInicio = document.getElementById('confGymPCrearHORAINICIO') as HTMLInputElement;
    const horaFinalizacion = document.getElementById('confGymPCrearHORAFINAL') as HTMLInputElement;
  }

  //Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (copia un calendario y sus actividades desde una fecha hasta otra)
  copiarCalendario(){
    const fInicio = document.getElementById('copCalenPDIA1') as HTMLInputElement;
    const fFinal = document.getElementById('copCalenPDIA2') as HTMLInputElement;
    const aMover = document.getElementById('copCalenPMOVIMIENTO') as HTMLInputElement;
  }

  //Función encargada de tomar los componentes mostrados en la pagina y enviarlos al api para llevar a cabo la función necesaria con los datos proporcionados (copia los datos correspondientes a un gimnasio y los añade a uno completamente nuevo)
  copiarGym(){
    const aCopiar = document.getElementById('copGympSELECT') as HTMLInputElement;
    const aPegar = document.getElementById('copGympNUEVO') as HTMLInputElement;
  }

  //Funcion encargada de tomar el identificador de un producto que aun no ha sido aprovado, y cambiarle el estado a aprovado
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
      console.log(data)
    })
  }

  logOut(){
    this.api.logout();
  }

}//bracket que cierras


