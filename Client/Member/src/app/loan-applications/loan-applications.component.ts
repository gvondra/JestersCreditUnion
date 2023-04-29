import { Component, OnInit } from '@angular/core';
import { LoanApplication } from '../models/loan-application';
import { LoanApplicationService } from '../services/loan-application.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-loan-applications',
  templateUrl: './loan-applications.component.html',
  styles: [
  ]
})
export class LoanApplicationsComponent implements OnInit {

  ErrorMessage: string | null = null;
  LoanApplications: LoanApplication[] | null = null;

  constructor(
    private loanApplicationService: LoanApplicationService
  ) { }

  ngOnInit(): void {
    this.LoanApplications = null;
    firstValueFrom(this.loanApplicationService.Search())
    .then(loanApplications => this.LoanApplications = loanApplications)
    .catch(err => {
      console.error(err);
    });
  }

  ToLocalDateString(value: string | null) : string {
    let result: string = "";
    if (value && value !== "") {
      let date: Date = new Date(Date.parse(value));
      result = date.toLocaleDateString();
    }
    return result;
  }

}
