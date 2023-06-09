import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import html2canvas from 'html2canvas';
import { jsPDF } from 'jspdf';
import * as pdfMake from 'pdfmake/build/pdfmake';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';
(pdfMake as any).vfs = pdfFonts.pdfMake.vfs;
import htmlToPdfmake from 'html-to-pdfmake';
declare var html2pdf: any; // Declaración del objeto html2pdf;
import { ActivatedRoute } from '@angular/router';
import { Params } from '@angular/router';
import {GetApiService} from "../get-api.service";
import { SharedService} from "../shared.service";



@Component({
  selector: 'app-reporte',
  templateUrl: './reporte.component.html',
  styleUrls: ['./reporte.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ReporteComponent implements OnInit{
  constructor(private route: ActivatedRoute, private api: GetApiService, private sharedService: SharedService) { }

  fecha = "";
  nombre = 'Sebastian Quesada';
  cedula = '118510858';
  direccion = '';
  birthdate = '';
  email = '';
  pais = '';
  max = '';
  ruta = '';

  //variables para la tabla
  titulos:string[] = ['TITULO1', 'TITULO2'];
  filas:string[][] = [];

  //esto es un ejemplo
  fila1:string[] = ['Fila1 V1', 'Fila1 V2'];
  //filas.push(fila1);


  ngOnInit(): void {
    const meses = [
      "enero",
      "febrero",
      "marzo",
      "abril",
      "mayo",
      "junio",
      "julio",
      "agosto",
      "septiembre",
      "octubre",
      "noviembre",
      "diciembre"
    ];
    const fechaActual = new Date();
    const year = fechaActual.getFullYear();
    const mes = meses[fechaActual.getMonth()];
    const dia = fechaActual.getDate();
    this.fecha = `${dia} de ${mes} de ${year}`;
    this.getInfoCliente();

    this.route.params.subscribe((params: Params) => {
      const info = this.sharedService.jsonData;
      console.log(info);
      this.titulos = info.titulos;
      this.filas = info.data;
      this.ruta = info.ruta;
      // Aquí puedes utilizar los valores de los parámetros como desees
    });;

    this.crearTabla(this.titulos, this.filas);

    //pantalla de carga poner
    this.generatePDF();
    this.api.ruta(this.ruta);
    //pantalla de carga quitar
  }

  crearTh(heads:string[]){
    const tr = document.createElement('tr');
    heads.forEach((head:string)=>{
      const th = document.createElement('th');
      th.scope = "col";
      th.className = "fs-sm text-dark text-uppercase-bold-sm px-0";
      th.innerText = head;
      tr.appendChild(th);
    })
    return tr;
  }

  crearContenido(contenido:string[][]){
    const listaTodo:HTMLTableRowElement[] = [];

    const largo = contenido.length;
    for (let i = 0; i < largo; i++) {
      const tr = document.createElement('tr');
      const infoLargo = contenido[i].length;
      for (let j = 0; j < infoLargo; j++){
        const td = document.createElement('td');
        td.className = "px-0";
        td.innerText = contenido[i][j];
        tr.appendChild(td);
      }
      listaTodo.push(tr);
    }
    return listaTodo;
  }

  crearTabla(heads:string[], contenido:string[][]){
    const table = document.createElement('table');
    table.className = "table border-bottom border-gray-200 mt-3";

    const thead = document.createElement('thead');
    thead.appendChild(this.crearTh(heads));
    table.appendChild(thead);

    const tbody = document.createElement('tbody');
    const cont = this.crearContenido(contenido);
    cont.forEach((row:HTMLTableRowElement) => {
      tbody.appendChild(row);
    })
    table.appendChild(tbody);



    const todo = document.getElementById('tabla') as HTMLDivElement;
    todo.append(table);
  }

  getInfoCliente(){
    let data = localStorage.getItem('usuario');
    // @ts-ignore
    data = JSON.parse(data);
    console.log(data);
    // @ts-ignore
    this.nombre = `${data.firstname} ${data.lastname1} ${data.lastname2}`;
    // @ts-ignore
    this.direccion = data.address;
    // @ts-ignore
    this.birthdate = data.birthdate;
    // @ts-ignore
    this.email = data.email;
    // @ts-ignore
    this.cedula = data.id;
     // @ts-ignore
    this.pais = data.country;
      // @ts-ignore
    this.max = data.maxconsumption;
  }

  generatePDF() {
    const reporte = document.getElementById('reporte') as HTMLDivElement;
    console.log(reporte);
    html2pdf().from(reporte).save();
  }






}
