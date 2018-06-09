import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { RoleRequest } from './role-request';
import { environment } from '../environments/environment';

@Injectable()
export class FormService {

  constructor(private http: Http) { }

  createRoleRequest(roleRequest: RoleRequest): Promise<any> {
    return this.http.post(environment.baseUrl + "Forms/RoleRequest", roleRequest,
    {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
  }

  getFormHtml(formId: string): Promise<string> {
    return this.http.get(environment.baseUrl + "Form/" + formId + "/html", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.text() as string)
  }

}
