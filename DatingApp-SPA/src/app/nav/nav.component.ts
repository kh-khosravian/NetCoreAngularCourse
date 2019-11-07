import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/AlertifyService.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login = () => {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('logged in successfully');
    }, err => {
      this.alertify.Error(err);
    });
  }

  loggedIn = () => {
    return this.authService.loggedIn();
  }
  logout = () => {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
  }

}
