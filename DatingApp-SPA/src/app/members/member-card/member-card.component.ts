import { Component, OnInit, Input } from '@angular/core';
import { UserModel } from 'src/app/_model/userModel';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/User.service';
import { AlertifyService } from 'src/app/_services/AlertifyService.service';


@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})

export class MemberCardComponent implements OnInit {

  @Input() user: UserModel;
  constructor(private auth: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  sendLike = (id: number) => {
    this.userService.SendLike(this.auth.currentUser.id, id).subscribe(() => {
      this.alertify.success('you have liked: ' + this.user.knownAs);
    }, e => {
      this.alertify.Error(e);
    })
  }

}
