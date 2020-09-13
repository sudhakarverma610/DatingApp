import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { User } from '../models/user';
import { UserService } from '../Services/user.service';
import { AlertifyService } from '../Services/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberListResolver implements Resolve<User[]>{
     
    constructor(private userService:UserService,private router:Router,private alertifyService:AlertifyService ) {
         
        
    }
    resolve(route: ActivatedRouteSnapshot):Observable<User[]> {
        return this.userService.getUsers().pipe(
            catchError(error=>{
                this.alertifyService.Error("Problem Retriving Data");
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }

}