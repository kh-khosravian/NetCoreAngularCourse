import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserModel } from '../_model/userModel';
import { UserService } from '../_services/User.service';
import { AlertifyService } from '../_services/AlertifyService.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_model/message';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MessageListResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'Unread';
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService
              , private authService: AuthService) { }

    resolve = (route: ActivatedRouteSnapshot): Observable<Message[]> => {
        return this.userService.GetMessages(this.authService.currentUser.id, this.pageNumber
                                          , this.pageSize, this.messageContainer).pipe(
            catchError(err => {
                this.alertify.Error('poblem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        )
    }

}