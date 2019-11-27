import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { UserModel } from '../_model/userModel';
import { PaginatedResult } from '../_model/paginationItem';
import { isFulfilled } from 'q';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'user/';

  constructor(private http: HttpClient) { }

  GetUserList = (page?, itemPerPage?, userparams?): Observable<PaginatedResult<UserModel[]>> => {
    const paginatedResult: PaginatedResult<UserModel[]> = new PaginatedResult<UserModel[]>();
    let params = new HttpParams();

    if (page != null && itemPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemPerPage);
    }

    if (userparams != null) {
      params = params.append('minAge', userparams.minAge)
      params = params.append('maxAge', userparams.maxAge)
      params = params.append('gender', userparams.gender)
      params = params.append('orderBy', userparams.orderBy)
      
    }
    return this.http.get<UserModel[]>(this.baseUrl, { observe: "response", params })
      .pipe(
        map(res => {
          paginatedResult.result = res.body;
          if (res.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(res.headers.get('Pagination'));
          }
          return paginatedResult;
        }));
  }

  GetUser = (id: number): Observable<UserModel> => {
    return this.http.get<UserModel>(this.baseUrl + id);
  }

  UpdateUser = (id: number, user: UserModel) => {
    return this.http.put(this.baseUrl + id, user);
  }

  setMainPhoto = (userId: number, id: number) => {
    return this.http.post(environment.apiUrl + 'users/' + userId + '/photos/' + id + '/setmain', {});
  }

  DeletePhoto = (userId: number, id: number) => {
    return this.http.delete(environment.apiUrl + 'users/' + userId + '/photos/' + id);
  }
}
