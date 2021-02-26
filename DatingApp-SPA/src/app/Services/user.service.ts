import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class UserService {
baseUrl = environment.baseUrl;
constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]>{
      return this.http.get<User[]>(`${this.baseUrl}User`);
  }

  getUser(id: number): Observable<User>{
    return this.http.get<User>(`${this.baseUrl}User/${id}`);
}
UpdateUser(id: number, user: User){
  return this.http.put(`${this.baseUrl}User/${id}`, user);
}
}
