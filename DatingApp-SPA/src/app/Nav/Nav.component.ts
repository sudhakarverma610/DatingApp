import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/Auth.service';

@Component({
  selector: 'app-Nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};
  constructor(private authService:AuthService) { }

  ngOnInit() {
  }
  Login(){
    console.log(this.model);
    this.authService.Login(this.model).subscribe(next=>{
      console.log("Login Successfully");
    },
    error=>{
      console.log("some Error Occured");
    })
  }
  LoggedIn(){
    const token=localStorage.getItem('token');
    return !!token;
  }
  LogOut(){
    localStorage.removeItem('token');
    console.log("logout user Successfully");
  }



}
