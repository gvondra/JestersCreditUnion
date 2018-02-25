import { Component, OnInit } from '@angular/core';
import { MembershipRequest } from '../membership-request';
@Component({
  selector: 'app-request-membership',
  templateUrl: './request-membership.component.html',
  styleUrls: ['./request-membership.component.css']
})
export class RequestMembershipComponent implements OnInit {

  membershipRequest : MembershipRequest = new MembershipRequest();

  constructor() { }

  ngOnInit() {
  }

}
