import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  confirm = (message: string, okCallBack: () => any) => {
    alertify.confirm(message, (e: any) => {
      okCallBack();
    });
  }

  success = (message: string) => {
    alertify.success(message);
  }

  warrning = (message: string) => {
    alertify.warrning(message);
  }

  Error = (message: string) => {
    alertify.error(message);
  }

  message = (message: string) => {
    alertify.message(message);
  }
}
