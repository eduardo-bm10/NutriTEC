import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import html2canvas from 'html2canvas';
import { jsPDF } from 'jspdf';

@Component({
  selector: 'app-reporte',
  templateUrl: './reporte.component.html',
  styleUrls: ['./reporte.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ReporteComponent implements OnInit{
  fecha = "";
  nombre = 'Sebastian Quesada';
  cedula = '118510858'

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
    this.fecha = `${dia} de ${mes} de ${year}`
  }

  async generatePDF() {
    const pdfContentEl = document.getElementById('reporte');

    const doc = new jsPDF();

    // @ts-ignore
    await doc.html(pdfContentEl.innerHTML).save('test.pdf');
  }


}
