import {Component, Renderer2, ElementRef, OnInit} from '@angular/core';
import { SharedService} from "./shared.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  constructor(private sharedService: SharedService) {}
  title = 'NutriTEC';
  mostrar: boolean = false;

  ngOnInit() {
    this.sharedService.mostrarSubject.subscribe((value: boolean) => {
      this.mostrar = value;
    });
  }
}
