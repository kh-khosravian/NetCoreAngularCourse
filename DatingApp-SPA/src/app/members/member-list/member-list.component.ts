import { Component, OnInit } from '@angular/core';
import { UserModel } from '../../_model/userModel';
import { UserService } from '../../_services/User.service';
import { AlertifyService } from '../../_services/AlertifyService.service';
import { ThrowStmt } from '@angular/compiler';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: UserModel[];

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data.users;
    });
  }

}