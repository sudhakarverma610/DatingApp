import { Component, OnInit,  Output, EventEmitter } from '@angular/core';
import { AuthService } from '../Services/Auth.service';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit{
  model:any={};
  @Output() cancelRegister=new EventEmitter();
  constructor(private authService:AuthService) { }

  ngOnInit() {
  }
  Register(){
    this.authService.Register(this.model).subscribe(res=>
      {
        console.log("User Register Successfully");
      },
      error=>{
        console.log("error"+error);
        console.log(error);
      })
  }
  Cancel(){
    console.log("canceled");
    this.cancelRegister.emit(false);
  }
}
