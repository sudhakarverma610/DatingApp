import { Component, OnInit } from '@angular/core';
import { User } from './models/user';
import { AuthService } from './Services/Auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  constructor(private authService: AuthService){}
  title = 'DatingApp-SPA';
  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const photoUrl = localStorage.getItem('photoUrl');

    if (token)
    {
      this.authService.decodedToken = this.authService.JwtHelper.decodeToken(token);
    }
    if (photoUrl){
      this.authService.photoUrl.next(photoUrl);
    }

  }

}
