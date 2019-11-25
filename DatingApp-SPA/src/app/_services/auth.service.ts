import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs/';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { UserModel } from '../_model/userModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseURL = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodeToken: any;
  currentUser: UserModel;
  PhotoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currnentPhotoUrl = this.PhotoUrl.asObservable();


  constructor(private http: HttpClient) { }

  changeMemberPhoto(photoUrl: string) {
    this.PhotoUrl.next(photoUrl);
  }

  login = (model: any) => {
    return this.http.post(this.baseURL + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodeToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          this.changeMemberPhoto(this.currentUser.photoURL);
        }
      })
    );
  }

  register = (model: any) => {
    return this.http.post(this.baseURL + 'register', model);
  }

  loggedIn = () => {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
