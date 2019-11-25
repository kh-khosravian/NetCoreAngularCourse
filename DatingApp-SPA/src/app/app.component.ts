import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserModel } from './_model/userModel';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  jwtHelper = new JwtHelperService();

  constructor(private auth: AuthService) { }

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      this.auth.decodeToken = this.jwtHelper.decodeToken(token);
    }
    const user: UserModel = JSON.parse(localStorage.getItem('user'));
    if (user) {
      this.auth.currentUser = user;
      this.auth.changeMemberPhoto(user.photoURL);
    }
  }
}
