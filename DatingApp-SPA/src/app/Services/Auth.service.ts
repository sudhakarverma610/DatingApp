import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import { BehaviorSubject, from } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseUrl = environment.baseUrl + 'Auth/';
JwtHelper = new JwtHelperService();
decodedToken: any;
photoUrl = new BehaviorSubject<string>('assets/img/avtars/default.png');
constructor(private httpClient: HttpClient) { }
Login(model: any){
  return this.httpClient.post(this.baseUrl + 'Login', model)
  .pipe(
    map((res: any) => {
      const user = res;
      if (user) {
        localStorage.setItem('token', user.token);
        const LoginUser = user.user as User;
        localStorage.setItem('photoUrl', LoginUser.photoUrl);
        this.photoUrl.next(LoginUser.photoUrl);
      }
      this.decodedToken = this.JwtHelper.decodeToken(user.token);
      console.log(this.decodedToken);
    })
  );
}
Register(model: any){
  return this.httpClient.post(this.baseUrl + 'register', model)
  .pipe(
    map((res: any) => {
      const user = res;
    })
  );
}
LoggedIn(){
    const token = localStorage.getItem('token');
   // this.photoUrl.next(localStorage.getItem('photoUrl'));
    return !this.JwtHelper.isTokenExpired(token);
}
}
