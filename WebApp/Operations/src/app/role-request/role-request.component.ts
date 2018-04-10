import { Component, OnInit } from '@angular/core';
import { RoleRequest } from '../role-request';
import { FormService } from '../form.service';

@Component({
  selector: 'app-role-request',
  templateUrl: './role-request.component.html',
  styleUrls: ['./role-request.component.css'],
  providers: [ FormService ]
})
export class RoleRequestComponent implements OnInit {

  roleRequest: RoleRequest;
  message: string;

  constructor(private formService: FormService) { }

  Submit() {
    this.formService.createRoleRequest(this.roleRequest)
    .then(response => this.message = "Request submitted.")
    .catch(err => this.message = err);
  }

  ngOnInit() {
    this.roleRequest = new RoleRequest();
    var p = JSON.parse(localStorage.getItem('profile'));    
    if (p) {
      this.roleRequest.FullName = p.name;
    }
  }

}
