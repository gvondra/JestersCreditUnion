import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { LoanApplication } from '../models/loan-application';
import { LoanApplicationService } from '../services/loan-application.service';
@Component({
  selector: 'app-loan-application',
  templateUrl: './loan-application.component.html',
  styles: [
  ]
})
export class LoanApplicationComponent implements OnInit {

  LoanApplication: LoanApplication = new LoanApplication();
  ErrorMessage: string | null = null;

  constructor(public loanApplicationService: LoanApplicationService) { }

  ngOnInit(): void {
    this.LoanApplication = new LoanApplication();
  }

  Save(): void {
    this.ErrorMessage = null;
    firstValueFrom(this.loanApplicationService.Create(this.LoanApplication))
    .then(app => this.LoanApplication = app)
    .catch(err => {
      console.error(err);
      this.ErrorMessage = err.message || "Unexpected Error"
    });
  }
}
