import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ToDoListComponent } from './todo/todo-list/todo-list.component';
import { ToDoEditComponent } from './todo/todo-edit/todo-edit.component';
import { ToDoDeleteComponent } from './todo/todo-delete/todo-delete.component';

const routes: Routes = [
  { path: '', redirectTo: '/todo', pathMatch: 'full' },
  { path: 'todo', component: ToDoListComponent },
  { path: 'todo/edit', component: ToDoEditComponent }, // Rota para criar nova tarefa
  { path: 'todo/edit/:id', component: ToDoEditComponent }, // Rota para editar tarefa 
  { path: 'todo/delete/:id', component: ToDoDeleteComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
