import { Component, OnInit } from '@angular/core';
import { UserModel } from '../_model/userModel';
import { PaginationItem, PaginatedResult } from '../_model/paginationItem';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/User.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/AlertifyService.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: UserModel[];
  PageModel: PaginationItem;
  likesParam: string;

  constructor(private authService: AuthService, private userService: UserService
            , private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.PageModel = data['users'].pagination;
    });
    this.likesParam = 'Likers';
  }
  pageChanged(event: any): void {
    this.PageModel.currentPage = event.page;
    this.loadUser();
  }

  loadUser = () => {
    this.userService.GetUserList(this.PageModel.currentPage, this.PageModel.itemsPerPage, null, this.likesParam)
      .subscribe((res: PaginatedResult<UserModel[]>) => {
        this.users = res.result;
        this.PageModel = res.pagination;
      }, e => {
        this.alertify.Error(e);
      });
  }
}
