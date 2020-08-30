import { Component, OnInit } from '@angular/core';
import { AuthService } from './Services/Auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  constructor(private authService:AuthService){}
  ngOnInit(): void {
    const token=localStorage.getItem("token");
    //console.log("token");
   // console.log(token);
    if(token)
    {
      this.authService.decodedToken=this.authService.JwtHelper.decodeToken(token);
      //console.log(this.authService.decodedToken);
    }
  }
  title = 'DatingApp-SPA';

}
