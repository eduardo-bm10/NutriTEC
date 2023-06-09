import {Component, OnInit} from '@angular/core';
import { GetApiService } from '../get-api.service';

@Component({
  selector: 'app-nutri',
  templateUrl: './nutri.component.html',
  styleUrls: ['./nutri.component.css']
})

export class NutriComponent implements OnInit {
  constructor(private api:GetApiService){}

  infoNutri = {};
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

  opcionesGlobales = {
    "pacientes" : {},
    "pacientesAsociados": {},
    "planes" : {}
  }

  ngOnInit() {
    for (let i = 0; i < this.pantallas.length; i++) {
      const tmp = document.getElementById(this.pantallas[i]) as HTMLInputElement
      tmp.style.display = 'none'
    }
    //this.cargarProvincias(["gestSucSpaPROVINCIA", "gestEmplPPROVINCIA", "gestEmplPPROVINCIA2"]);
    this.cargarCheckBoxesComidas();
    this.cargarPacientes();
    this.cargarPlanes();
    const calorias = document.getElementById('gestionPlanesCaloriasExistente') as HTMLInputElement;
    calorias.value = String(this.totalCalorias);
    const data = localStorage.getItem('usuario');
    // @ts-ignore
    const info = JSON.parse(data);
    this.infoNutri = info;
    this.nombre = `Nutricionista Lic. ${info['firstname']} ${info['lastname1']}`
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

  cargarPacientes(){
    this.api.getPatients().subscribe((patients) => {
      var llegada = JSON.parse(JSON.stringify(patients));
      //llegada = llegada.filter(item => otroJson.ids.includes(item.id));
      console.log(llegada);
      this.opcionesGlobales.pacientes = llegada;
      const tmpTotal = document.getElementById("busquedaAsociacionClientesComoPacientesSELECT") as HTMLInputElement;
      const tmpTotal2 = document.getElementById("asignacionPlanPaciente") as HTMLInputElement;
      const tmpTotal3 = document.getElementById("seguimientoPacienteSELECT") as HTMLInputElement;

      this.api.getPatientNutriotionistAssociation().subscribe(associationsLlegada => {
        var associations = JSON.parse(JSON.stringify(associationsLlegada));

        
        
        const nutritionist = localStorage.getItem("usuario")
        if (nutritionist !== null) {
          const nutritionistAux = JSON.parse(nutritionist)

          var id_patients = [];
          var id_patients_differ = [];
          for (const op in associations) { 
            if (associations[op].nutritionistid === nutritionistAux.id) {
              id_patients.push(associations[op].patientid)
            } else {
              id_patients_differ.push(associations[op].patientid)
            }
          }

          for(const op in llegada){
            if (id_patients.includes(llegada[op].id)) {
              const aux = llegada[op];
              const opcionTmp = document.createElement('option');
              opcionTmp.value = aux.id;
              opcionTmp.textContent = aux.id;
              tmpTotal2.appendChild(opcionTmp);
            }
          }

          for(const op in llegada){
            if (!id_patients.includes(llegada[op].id) && !id_patients_differ.includes(llegada[op].id)) {
              const aux = llegada[op];
              const opcionTmp = document.createElement('option');
              opcionTmp.value = aux.id;
              opcionTmp.textContent = aux.id;
              tmpTotal.appendChild(opcionTmp);
            }
          }
          
    
          for(const op in llegada){
            if (id_patients.includes(llegada[op].id)) {
              const aux = llegada[op];
              const opcionTmp = document.createElement('option');
              opcionTmp.value = aux.id;
              opcionTmp.textContent = aux.id;
              tmpTotal3.appendChild(opcionTmp);
            }
          }
        }
        
        this.cambiarInfo('productos', 'busquedaAsociacionClientesComoPacientesSELECT', 'TEST', 'TEST');
        this.cambiarInfo('productos', 'seguimientoPacienteSELECT', 'TEST', 'TEST');
        this.cambiarInfo('productos', 'asignacionPlanPaciente', 'TEST', 'TEST');
      })

    })
  }

  cargarPlanes(){
    this.api.getPlans().subscribe((data) => {
      const llegada = JSON.parse(JSON.stringify(data));
      console.log(llegada);
      this.opcionesGlobales.pacientes = llegada;
      const tmpTotal = document.getElementById("gestionPlanesSELECT") as HTMLInputElement;
      const tmpTotal2 = document.getElementById("asignacionPlanPlan") as HTMLInputElement;


      for(const op in llegada){
        const aux = llegada[op];
        const opcionTmp = document.createElement('option');
        opcionTmp.value = aux.id;
        opcionTmp.textContent = aux.id;
        tmpTotal.appendChild(opcionTmp);
      }

      for(const op in llegada){
        const aux = llegada[op];
        const opcionTmp = document.createElement('option');
        opcionTmp.value = aux.id;
        opcionTmp.textContent = aux.id;
        tmpTotal2.appendChild(opcionTmp);
      }

      this.cambiarInfo('productos', 'gestionPlanesSELECT', 'TEST', 'TEST');
      this.cambiarInfo('productos', 'asignacionPlanPlan', 'TEST', 'TEST');
    })
  }

  asignarPlanPaciente() {
    //const 
    const planCliente = document.getElementById("asignacionPlanPaciente") as HTMLInputElement;
    const planPlan = document.getElementById("asignacionPlanPlan") as HTMLInputElement;
    const planTiempo = document.getElementById("asignacionPlanTiempo") as HTMLInputElement;

    const cliente = planCliente.value;
    const plan = parseInt(planPlan.value);
    const tiempo = planTiempo.value;

    const fechaActualString = new Date().toISOString().substring(0, 10);

    const fechaFinalizacion = new Date();
    fechaFinalizacion.setDate(fechaFinalizacion.getDate() + parseInt(tiempo));
    const fechaFinalizacionString = fechaFinalizacion.toISOString().substring(0, 10);

    const nutricionista = localStorage.getItem("usuario")
    if (nutricionista !== null) {
      var nutricionistaId = JSON.parse(nutricionista).id
    }

    this.api.createPlanPatientAssociation(nutricionistaId,plan,cliente,fechaActualString,fechaFinalizacionString).subscribe((data) => {
      console.log(data)
    })
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

  crearNuevoProducto(){
    const barras = document.getElementById("gestionProductosBarras") as HTMLInputElement
    const descripcion = document.getElementById("gestionProductosDescripcion") as HTMLInputElement
    const hierro = document.getElementById("gestionProductosHierro") as HTMLInputElement
    const sodio = document.getElementById("gestionProductosSodio") as HTMLInputElement
    const energia = document.getElementById("gestionProductosEnergia") as HTMLInputElement
    const grasa = document.getElementById("gestionProductosGrasa") as HTMLInputElement
    const calcio = document.getElementById("gestionProductosCalcio") as HTMLInputElement
    const carbohidratos = document.getElementById("gestionProductosCarbohidratos") as HTMLInputElement
    const proteina = document.getElementById("gestionProductosProteina") as HTMLInputElement
    const vitaminasNOLISTO = document.getElementById("gestionProductosVitaminas") as HTMLSelectElement

    const vitaminas: string[] = [];
    

    for (let i = 0; i < vitaminasNOLISTO.options.length; i++) {
      const option = vitaminasNOLISTO.options[i];
      if (option.selected) {
        vitaminas.push(option.value);
      }
    }
    console.log(vitaminas)
    
    const vitaminasString: string = vitaminas.toString();

    
    this.api.createProduct(Number(barras.value), descripcion.value, Number(hierro.value), Number(sodio.value), Number(energia.value), Number(grasa.value), Number(calcio.value), Number(carbohidratos.value), Number(proteina.value), vitaminasString).subscribe((data) => {
      alert("swagger")
    })
  }

  asociarPacienteNutri(){
    const paciente = document.getElementById("busquedaAsociacionClientesComoPacientesSELECT") as HTMLInputElement
    // @ts-ignore
    const nutri =  localStorage.getItem('usuario');
    if (nutri !== null) {
      var nutricionistaId = JSON.parse(nutri).id
    }
    //esta linea de aca arriba tiene que ser el id del nutri que se logro

    this.api.createPatientNutrionistAssociation(nutricionistaId, paciente.value).subscribe((data) => {
      console.log(data)
    })
  }
  
  enviarRetroAlimentacion(){
    const mensaje = document.getElementById("seguimientoPacienteRetroalimentacion") as HTMLTextAreaElement
    const paciente = document.getElementById("seguimientoPacienteSELECT") as HTMLInputElement
    const nutri =  localStorage.getItem('usuario');
    if (nutri !== null) {
      var nutricionistaId = JSON.parse(nutri).id
    }

    this.api.createFeedback(nutricionistaId, paciente.value, mensaje.value).subscribe(data => {
      console.log(data)
    })
  }

  cargarRetroAlimentacion(){
    const paciente = document.getElementById("seguimientoPacienteSELECT") as HTMLInputElement
    console.log(paciente.value)

    this.api.getMeasurementById(Number(paciente.value)).subscribe(data => {
      const mensaje = document.getElementById("seguimientoPacienteRetroalimentacion") as HTMLTextAreaElement
      console.log(data)
    })
  }

  test(){
    console.log("resultado test:")
    this.api.getFeedbackSsn(333333333).subscribe(data => {
      console.log(data)
    })
  }

  logOut(){
    this.api.logout();
  }
}
