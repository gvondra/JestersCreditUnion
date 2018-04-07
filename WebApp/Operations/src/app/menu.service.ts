import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { MenuItem } from './menu-item';

@Injectable()
export class MenuService {

  constructor(private http: Http) { }

  getMenuItems() : Promise<Array<Array<MenuItem>>> {
    let items = JSON.parse(sessionStorage.getItem('jcuoMenuItems'));
    if (items) {
      return Promise.resolve(items as Array<Array<MenuItem>>);
    }
    else {
      return this.http.get(environment.baseUrl + "Menu", {
        headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
      })
      .toPromise()
      .then(response => {
        sessionStorage.setItem('jcuoMenuItems', JSON.stringify(response.json()));
        return response.json();
      })
    }    
  }
}
