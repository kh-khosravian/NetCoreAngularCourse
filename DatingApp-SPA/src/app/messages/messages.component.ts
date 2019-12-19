import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/User.service';
import { AlertifyService } from '../_services/AlertifyService.service';
import { ActivatedRoute } from '@angular/router';
import { Message } from '../_model/message';
import { PaginationItem, PaginatedResult } from '../_model/paginationItem';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: Message[];
  PageModel: PaginationItem;
  messageContainer = 'Unread';

  constructor(private userService: UserService, private authService: AuthService
    , private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data['messages'].result;
      this.PageModel = data['messages'].pagination;
    });
  }

  loadMessages = () => {
    this.userService.GetMessages(this.authService.currentUser.id, this.PageModel.currentPage
      , this.PageModel.itemsPerPage, this.messageContainer).subscribe((data: PaginatedResult<Message[]>) => {
        this.messages = data.result;
        this.PageModel = data.pagination;
      }, e => {
        this.alertify.Error(e);
      });
  }

  pageChanged = (event: any): void => {
    this.PageModel.currentPage = event.page;
    this.loadMessages();
  }
}
