import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/Auth.service';
import { AlertifyService } from '../Services/Alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  UserName: string;
  photoUrl;
  constructor(public authService: AuthService, private alertifyService: AlertifyService, private router: Router) { }

  ngOnInit() {
      this.UserName = this.authService.decodedToken?.unique_name;
      this.authService.photoUrl.subscribe(url => {
        this.photoUrl = url;
      });
  }
  Login(){
    console.log(this.model);
    this.authService.Login(this.model).subscribe(next => {
      this.UserName = this.authService.decodedToken?.unique_name;
      this.alertifyService.Success('Login Successfully');
      // console.log("Login Successfully");
    },
    error => {
      this.alertifyService.Error('UserName and Password are InValid ');
      // console.log("some Error Occured");
    },
    () => {
      this.router.navigate(['/members']);
    });
  }
  LoggedIn(){
    return this.authService.LoggedIn();
  }
  LogOut(){
    localStorage.removeItem('token');
    localStorage.removeItem('photoUrl');
    this.alertifyService.Message('Logout user Successfully');
    // console.log("logout user Successfully");
    this.router.navigate(['/home']);
  }

}
