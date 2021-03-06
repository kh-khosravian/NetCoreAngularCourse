import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/AlertifyService.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  photoUrl: string;

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.authService.currnentPhotoUrl.subscribe(photoUrl => { this.photoUrl = photoUrl; })
  }

  login = () => {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('logged in successfully');
    }, err => {
      this.alertify.Error(err);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn = () => {
    return this.authService.loggedIn();
  }
  logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodeToken = null;
    this.authService.currentUser = null;
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }

}
