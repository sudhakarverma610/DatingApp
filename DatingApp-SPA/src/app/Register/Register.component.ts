import { Component, OnInit,  Output, EventEmitter } from '@angular/core';
import { AuthService } from '../Services/Auth.service';
import { AlertifyService } from '../Services/Alertify.service';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit{
  model: any = {};

  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  @Output() cancelRegister = new EventEmitter();
  constructor(private authService: AuthService, private alertifyService: AlertifyService,
              private fb: FormBuilder,
              private router: Router ){ }
  ngOnInit() {
    // this.registerForm = new FormGroup({
    //   username: new FormControl('', [Validators.required]),
    //   password: new FormControl('',
    //   [Validators.required, Validators.minLength(4), Validators.maxLength(14)]),
    //   confirmPassword: new FormControl('', Validators.required)
    // }, this.PasswordMatcherValidator);
    this.createRegisterForm();
    this.bsConfig = Object.assign({}, { containerClass: 'theme-default' });

  }
  createRegisterForm(){
    this.registerForm = this.fb.group({
      gender: ['male'],
      knowAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      username: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(14)]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }
    , {validators: this.PasswordMatcherValidator});
  }
  PasswordMatcherValidator(g: FormGroup){
    return g.get('password').value === g.get('confirmPassword').value ? null : {mismatch: true};
  }

  Register(){
    console.log(this.registerForm.value);
    if (this.registerForm.valid){
      const user = Object.assign({}, this.registerForm.value);
      this.authService.Register(user).subscribe(res =>
      {
        this.alertifyService.Success('User Register Successfully');
      },
      error => {
        this.alertifyService.Error('Unable to Register');
        console.log(error);
      },
      () => {
        this.authService.Login(user).subscribe(res => {
          this.router.navigate(['/members']);
        });
      });
    }
    // this.authService.Register(this.model).subscribe(res =>
    //   {
    //     this.alertifyService.Success('User Register Successfully');
    //   },
    //   error => {
    //     this.alertifyService.Error('Unable to Register');
    //     console.log(error);
    //   });
  }
  Cancel(){
    // console.log("canceled");
    this.cancelRegister.emit(false);
  }

}
