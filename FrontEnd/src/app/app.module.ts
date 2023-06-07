import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ClienteComponent } from './cliente/cliente.component';
import { AdminComponent } from './admin/admin.component';
import { NutriComponent } from './nutri/nutri.component';
import {HttpClientModule} from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { LoginNUTRIComponent } from './login-nutri/login-nutri.component';
import { LoginADMINComponent } from './login-admin/login-admin.component';
import { NavVarComponent } from './nav-var/nav-var.component';
import { ReporteComponent } from './reporte/reporte.component';
import { CargaComponent } from './carga/carga.component';

const appRoutes:Routes=[
  {path:'', component: LoginComponent},
  {path:'paciente', component:ClienteComponent},
  {path:'admin', component:AdminComponent},
  {path:'nutri', component:NutriComponent},
  {path:'login-admin', component:LoginADMINComponent},
  {path:'login-nutri', component:LoginNUTRIComponent},
  {path:'prueba', component:ReporteComponent},
  {path:'**', component: NotFoundComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NotFoundComponent,
    ClienteComponent,
    AdminComponent,
    NutriComponent,
    LoginNUTRIComponent,
    LoginADMINComponent,
    NavVarComponent,
    ReporteComponent,
    CargaComponent
  ],
  imports: [
    RouterModule.forRoot(appRoutes),
    FormsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
