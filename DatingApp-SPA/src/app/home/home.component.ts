import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerModel=false;
  constructor() { }

  ngOnInit() {
  }
  ToggleRegister(){
    this.registerModel=!this.registerModel;
  }
  CancelRegister(registerModel:boolean){
    this.registerModel=registerModel;
  }
}
