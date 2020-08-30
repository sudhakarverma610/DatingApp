import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/Auth.service';
import { AlertifyService } from '../Services/Alertify.service';

@Component({
  selector: 'app-Nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};
  UserName:string;
  constructor(private authService:AuthService,private alertifyService:AlertifyService) { }

  ngOnInit() {    
      this.UserName=this.authService.decodedToken.unique_name;

  }
  Login(){
    console.log(this.model);
    this.authService.Login(this.model).subscribe(next=>{
      this.UserName=this.authService.decodedToken.unique_name;
      this.alertifyService.Success("Login Successfully");
      //console.log("Login Successfully");
    },
    error=>{
      this.alertifyService.Error("UserName and Password are InValid ");
      //console.log("some Error Occured");
    })
  }
  LoggedIn(){
    return this.authService.LoggedIn();
  }
  LogOut(){
    localStorage.removeItem('token');
    this.alertifyService.Message("Logout user Successfully");
    //console.log("logout user Successfully");
  }
 
}
