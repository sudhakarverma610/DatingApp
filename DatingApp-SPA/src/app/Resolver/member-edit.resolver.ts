import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { User } from '../models/user';
import { UserService } from '../Services/user.service';
import { AlertifyService } from '../Services/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../Services/Auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User>{

    constructor(private userService: UserService,
                private router: Router,
                private alertifyService: AlertifyService,
                private authService: AuthService ) {


    }
    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertifyService.Error('Problem Retriving Your data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }

}
