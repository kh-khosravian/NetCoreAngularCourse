import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserModel } from '../_model/userModel';
import { UserService } from '../_services/User.service';
import { AlertifyService } from '../_services/AlertifyService.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<UserModel> {

    jwtHelper = new JwtHelperService();
    constructor(private authService: AuthService, private userService: UserService,
                private router: Router, private alertify: AlertifyService) { }

    resolve = (route: ActivatedRouteSnapshot): Observable<UserModel> => {
        return this.userService.GetUser(this.authService.decodeToken.nameid).pipe(
            catchError(err => {
                this.alertify.Error('poblem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        )
    }

}