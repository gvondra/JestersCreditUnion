import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { User } from './user';
import { environment } from '../environments/environment';
@Injectable()
export class UserService {

  constructor(private http: Http) { }

  Search(searchText: string): Promise<Array<User>> { 
    let query = new URLSearchParams();
    query.append("s", searchText);
    return this.http.get(environment.baseUrl + "Users/Search", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`}),
      params: query
    })
    .toPromise()
    .then(response => response.json());
  }

  getUser(userId: string) : Promise<User> {
    return this.http.get(environment.baseUrl + "User/" + userId, {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as User)
  }

  putUser(user: User): Promise<User> {
    return this.http.put(environment.baseUrl + "User", user,
    {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as User)
  }
  
}
