import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToDoService } from '../services/todo.service';

@Component({
  selector: 'app-todo-delete',
  templateUrl: './todo-delete.component.html',
  styleUrls: ['./todo-delete.component.scss']
})
export class ToDoDeleteComponent implements OnInit {
  id: number | undefined;

  constructor(
    private todoService: ToDoService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = +params['id']; // Garante que o id seja inicializado a partir dos parâmetros da rota
      if (!this.id) {
        console.error('ID da tarefa é obrigatório para exclusão.');
        this.router.navigate(['/todo']); // Redireciona se o id não for fornecido
      }
    });
  }

  deleteToDo(): void {
    if (this.id) { // Verifica se o id está definido antes de tentar excluir
      this.todoService.deleteToDo(this.id).subscribe(
        () => {
          alert('Tarefa excluída com sucesso!');
          this.router.navigate(['/todo']);
        },
        erro => {
          console.error('Erro ao excluir tarefa', erro);
          alert('Erro ao excluir tarefa.');
        }
      );
    }
  }

  cancelar(): void {
    this.router.navigate(['/todo']);
  }
}
