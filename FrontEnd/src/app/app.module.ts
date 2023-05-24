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

const appRoutes:Routes=[
  {path:'', component: LoginComponent},
  {path:'cliente', component:ClienteComponent},
  {path:'admin', component:AdminComponent},
  {path:'nutri', component:NutriComponent},
  {path:'**', component: NotFoundComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NotFoundComponent,
    ClienteComponent,
    AdminComponent,
    NutriComponent
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
