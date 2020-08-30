import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators'
import { from } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseUrl="http://localhost:5000/api/Auth/"
constructor(private httpClient:HttpClient) { }
Login(model:any){
  return this.httpClient.post(this.baseUrl+"Login",model)
  .pipe(
    map((res:any)=>{
      const user=res;
      if(user)
        localStorage.setItem("token",user.token);
    })
  )
}
Register(model:any){
  return this.httpClient.post(this.baseUrl+"register",model)
  .pipe(
    map((res:any)=>{
      const user=res; 
    })
  )
}
}
