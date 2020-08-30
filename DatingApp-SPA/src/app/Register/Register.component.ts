import { Component, OnInit,  Output, EventEmitter } from '@angular/core';
import { AuthService } from '../Services/Auth.service';
import { AlertifyService } from '../Services/Alertify.service';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit{
  model:any={};
  @Output() cancelRegister=new EventEmitter();
  constructor(private authService:AuthService,private alertifyService:AlertifyService ){ }

  ngOnInit() {
  }
  Register(){
    this.authService.Register(this.model).subscribe(res=>
      {
        this.alertifyService.Success("User Register Successfully");
        //console.log("User Register Successfully");
      },
      error=>{
        this.alertifyService.Error("Unable to Register"); 
       // console.log("error"+error);
        console.log(error);
      })
  }
  Cancel(){
    //console.log("canceled");
    this.cancelRegister.emit(false);
  }
 
}
