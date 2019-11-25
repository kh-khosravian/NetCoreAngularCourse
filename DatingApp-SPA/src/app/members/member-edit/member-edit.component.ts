import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserModel } from 'src/app/_model/userModel';
import { AlertifyService } from 'src/app/_services/AlertifyService.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/User.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm', { static: true }) editFrom: NgForm;
  user: UserModel;
  @HostListener('window:beforeunload', ['$event'])

  unloadNotification($event: any) {
    if (this.editFrom.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(private route: ActivatedRoute, private alertify: AlertifyService, private userService: UserService,
    private authService: AuthService) { }
  ngOnInit() {
    this.route.data.subscribe(userData => {
      this.user = userData['user'];
    });
  }

  updateUser = () => {
    this.userService.UpdateUser(this.authService.decodeToken.nameid, this.user).subscribe(next => {
      this.alertify.success('profile updated');
      this.editFrom.reset(this.user);
    }, err => {
      this.alertify.success(err);
    })

  }
  UpdateMainUserPhoto = (photoUrl) => {
    this.user.photoURL = photoUrl;
    this.authService.changeMemberPhoto(photoUrl);
  }
}
