import { Component, OnInit } from '@angular/core';
import { MembershipRequest } from '../membership-request';
import { LookupItem } from "../lookup-item";
import { LookupService } from "../lookup.service";
@Component({
  selector: 'app-request-membership',
  templateUrl: './request-membership.component.html',
  styleUrls: ['./request-membership.component.css'],
  providers: [LookupService]
})
export class RequestMembershipComponent implements OnInit {

  membershipRequest: MembershipRequest = new MembershipRequest();
  genders: LookupItem[] = null;
  occupancyStatuses: LookupItem[] = null;
  employmentStatuses: LookupItem[] = null;
  constructor(private lookupService: LookupService) { }

  ngOnInit() {          
    this.lookupService.getActiveLookup("Gender", "")
    .then(data => this.genders = data)
    this.lookupService.getActiveLookup("OccupancyStatus", "")
    .then(data => this.occupancyStatuses = data)
    this.lookupService.getActiveLookup("EmploymentStatus", "")
    .then(data => this.employmentStatuses = data)    
  }

}
