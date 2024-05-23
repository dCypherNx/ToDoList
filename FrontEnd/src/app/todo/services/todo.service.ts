import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ToDo } from '../models/todo.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ToDoService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  listToDos(): Observable<ToDo[]> {
    return this.http.get<ToDo[]>(this.apiUrl+"/ToDo");
  }

  getToDo(id: number): Observable<ToDo> {
    return this.http.get<ToDo>(`${this.apiUrl+"/ToDo"}/${id}`);
  }

  createToDo(obj: ToDo): Observable<ToDo> {
    var ret = this.http.post<ToDo>(this.apiUrl+"/ToDo", obj);
    return ret;
  }

  updateToDo(id: number, obj: ToDo): Observable<ToDo> {
    return this.http.put<ToDo>(`${this.apiUrl+"/ToDo"}/${id}`, obj);
  }

  deleteToDo(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl+"/ToDo"}/${id}`);
  }
}
