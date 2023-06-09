import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor() { }

  mostrar: boolean = false;
  jsonData: any = {};
  mostrarSubject: Subject<boolean> = new Subject<boolean>();

  setMostrar(value: boolean): void {
    this.mostrar = value;
    this.mostrarSubject.next(value);
  }
}
