import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserModel } from '../_model/userModel';
import { UserService } from '../_services/User.service';
import { AlertifyService } from '../_services/AlertifyService.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class memberListResolver implements Resolve<UserModel[]> {
    pageNumber = 1;
    pageSize = 5;
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) { }

    resolve = (route: ActivatedRouteSnapshot): Observable<UserModel[]> => {
        return this.userService.GetUserList(this.pageNumber, this.pageSize).pipe(
            catchError(err => {
                this.alertify.Error('poblem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        )
    }

}