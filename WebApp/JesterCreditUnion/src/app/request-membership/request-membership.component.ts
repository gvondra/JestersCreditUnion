import { Component, OnInit } from '@angular/core';
import { MembershipRequest } from '../membership-request';
import { LookupItem } from "../lookup-item";
import { LookupService } from "../lookup.service";
import { ConfigurationService } from "../configuration.service";
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
  constructor(private configurationService: ConfigurationService,
              private lookupService: LookupService) { }

  ngOnInit() {
    this.configurationService.getLookupServiceUrl()
    .then(url => {      
      this.lookupService.getActiveLookup("Gender", "", url)
      .then(data => this.genders = data)
      this.lookupService.getActiveLookup("OccupancyStatus", "", url)
      .then(data => this.occupancyStatuses = data)
      this.lookupService.getActiveLookup("EmploymentStatus", "", url)
      .then(data => this.employmentStatuses = data)
    })
  }

}
