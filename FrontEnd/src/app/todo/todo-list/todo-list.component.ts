import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToDoService } from '../services/todo.service';
import { ToDo } from '../models/todo.model';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class ToDoListComponent implements OnInit {
  todos: ToDo[] = [];
  loadError: boolean = false;

  constructor(
    private toDoService: ToDoService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.listToDos();
  }

  listToDos(): void {
    this.toDoService.listToDos().subscribe(
      (received: ToDo[]) => {
        this.todos = received;
      },
      (erro) => {
        console.error('Erro ao carregar tarefas', erro);
        this.loadError = true;
      }
    );
  }

  editToDo(id: number | undefined): void {
    if (id !== undefined) {
      this.router.navigate(['/todo/edit', id]);
    }
  }

  deleteToDo(id: number | undefined): void {
    if (id !== undefined) {
      if (confirm('Tem certeza que deseja excluir esta tarefa?')) {
        this.toDoService.deleteToDo(id).subscribe(
          () => {
            alert('Tarefa excluída com sucesso!');
            this.listToDos(); // Atualiza a lista após a exclusão
          },
          (erro) => {
            console.error('Erro ao excluir tarefa', erro);
            alert('Erro ao excluir tarefa.');
          }
        );
      }
    }
  }

  fmtDate(date: Date | undefined): string {
    if (!date) {
      return ''; // Retorna uma string vazia se a data for undefined
    }

    const objDate = new Date(date);
    const day = objDate.getDate().toString().padStart(2, '0');
    const month = (objDate.getMonth() + 1).toString().padStart(2, '0');
    const year = objDate.getFullYear();
    return `${day}/${month}/${year}`;
  }
}
