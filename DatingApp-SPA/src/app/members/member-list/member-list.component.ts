import { Component, OnInit } from '@angular/core';
import { UserModel } from '../../_model/userModel';
import { UserService } from '../../_services/User.service';
import { AlertifyService } from '../../_services/AlertifyService.service';
import { ThrowStmt } from '@angular/compiler';
import { ActivatedRoute } from '@angular/router';
import { PaginationItem, PaginatedResult } from 'src/app/_model/paginationItem';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: UserModel[];
  user: UserModel = JSON.parse(localStorage.getItem('user'));
  genders: [{ value: 'male', display: 'male' }, { value: 'female', display: 'female' }];
  userParam: any = {};
  PageModel: PaginationItem;
  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) {

  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.PageModel = data['users'].pagination;
    });
    this.userParam.gender = this.user.gender === 'female' ? 'male' : 'femaile';
    this.userParam.minAge = 18;
    this.userParam.maxAge = 99;
    this.userParam.orderBy = 'lastActive';
  }
  resetFilter = () => {
    this.userParam.gender = this.user.gender === 'female' ? 'male' : 'femaile';
    this.userParam.minAge = 18;
    this.userParam.maxAge = 99;
    this.userParam.orderBy = 'lastActive';
    this.loadUser();
  }

  pageChanged(event: any): void {
    this.PageModel.currentPage = event.page;
    this.loadUser();
  }

  loadUser = () => {
    this.userService.GetUserList(this.PageModel.currentPage, this.PageModel.itemsPerPage, this.userParam)
      .subscribe((res: PaginatedResult<UserModel[]>) => {
        this.users = res.result;
        this.PageModel = res.pagination;
      }, e => {
        this.alertify.Error(e);
      });
  }
}
