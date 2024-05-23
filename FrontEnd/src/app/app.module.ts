import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ToDoListComponent } from './todo/todo-list/todo-list.component';
import { ToDoEditComponent } from './todo/todo-edit/todo-edit.component';
import { ToDoDeleteComponent } from './todo/todo-delete/todo-delete.component';
import { ToDoService } from './todo/services/todo.service';

import { StatusPipe } from './pipes/status.pipe'; // Importação do novo pipe

@NgModule({
  declarations: [
    AppComponent,
    ToDoListComponent,
    ToDoEditComponent,
    ToDoDeleteComponent,
    StatusPipe
    // Adicione outros componentes aqui conforme necessário
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
    // Adicione outros módulos aqui conforme necessário
  ],
  providers: [
    ToDoService
    // Adicione outros serviços aqui conforme necessário
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
