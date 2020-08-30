import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'; 
import { NavComponent } from './Nav/Nav.component';
import { AppComponent } from './app.component';
import {FormsModule} from '@angular/forms';
import { AuthService } from './Services/Auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './Register/Register.component';
import { ErrorInterceptorProvider } from './Services/Error.Interceptor';

@NgModule({
  declarations: [				
    AppComponent, 
      NavComponent,
      HomeComponent,
      RegisterComponent
   ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
