import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToDoService } from '../services/todo.service';
import { ToDo } from '../models/todo.model';
import { isUndefined } from 'util';

@Component({
  selector: 'app-todo-edit',
  templateUrl: './todo-edit.component.html',
  styleUrls: ['./todo-edit.component.scss']
})
export class ToDoEditComponent implements OnInit {
  todoForm: FormGroup;
  todo: ToDo | undefined;
  id: number | undefined;

  constructor(
    private formBuilder: FormBuilder,
    private toDoService: ToDoService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.todoForm = this.formBuilder.group({
      description: ['', Validators.required],
      date: ['', Validators.required],
      status: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = params['id'] ? +params['id'] : undefined;
      if (typeof(this.id) !== 'undefined' ) {
        this.todoForm.get('description')?.disable();
        this.todoForm.get('date')?.disable();
        this.toDoService.getToDo(this.id).subscribe(todo => {
          if (todo.date) {
            const dateString = todo.date.toString().split('T')[0];
            this.todoForm.patchValue({ ...todo, date: dateString });
          } else {
            this.todoForm.patchValue(todo);
          }
        });
      }
    });
  }

  saveToDo(): void {
    if (this.todoForm.valid) {
      if (this.id) {
        // Atualizar tarefa existente
        this.toDoService.updateToDo(this.id, this.todoForm.value).subscribe(
          () => {
            alert('Tarefa atualizada com sucesso!');
            this.router.navigate(['/todo']);
          },
          erro => {
            console.error('Erro ao atualizar tarefa', erro);
            alert('Erro ao atualizar tarefa.');
          }
        );
      } else {
        // Criar nova tarefa
        this.toDoService.createToDo(this.todoForm.value).subscribe(
          () => {
            alert('Tarefa criada com sucesso!');
            this.router.navigate(['/todo']);
          },
          erro => {
            console.error('Erro ao criar tarefa', erro);
            alert('Erro ao criar tarefa.');
          }
        );
      }
    }
  }
}
