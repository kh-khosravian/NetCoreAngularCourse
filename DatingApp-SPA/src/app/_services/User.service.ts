import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { UserModel } from '../_model/userModel';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'user/';

  constructor(private http: HttpClient) { }

  GetUserList = (): Observable<UserModel[]> => {
    return this.http.get<UserModel[]>(this.baseUrl);
  }

  GetUser = (id: number): Observable<UserModel> => {
    return this.http.get<UserModel>(this.baseUrl + id);
  }

  UpdateUser = (id: number, user: UserModel) => {
    return this.http.put(this.baseUrl + id, user);
  }

}
