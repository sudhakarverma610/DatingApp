import { Injectable } from "@angular/core";
import { CanActivate, Router } from '@angular/router'; 
import { AuthService } from '../Services/Auth.service';
import { AlertifyService } from '../Services/Alertify.service';
@Injectable({
    providedIn:'root'
})
export class AuthGuard implements CanActivate{
    /**
     *
     */
    constructor(private authService:AuthService,
        private alertifyService:AlertifyService,
        private router:Router) {
         
        
    }
    canActivate(): boolean {
        if(this.authService.LoggedIn())
        return true;
        this.alertifyService.Error("you should not pass!!!!");
        this.router.navigate(['/home']);
    }

}