import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Address } from '../models/address';
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
  Saving: boolean = false;
  InputColumnClass: string = "col-md-4"
  BorrowerBirthDate: string | null = null;
  BorrowerHireDate: string | null = null;

  constructor(public loanApplicationService: LoanApplicationService) { }

  ngOnInit(): void {
    this.LoanApplication = new LoanApplication();
    this.InitializeLoanApplication(this.LoanApplication);
  }

  private InitializeLoanApplication(loanApplication: LoanApplication): void {
    if (loanApplication.BorrowerBirthDate) {
      let birth: Date = new Date(loanApplication.BorrowerBirthDate);
      this.BorrowerBirthDate = `${birth.getFullYear()}-${(birth.getMonth()+1).toString().padStart(2, "0")}-${birth.getDate().toString().padStart(2, "0")}`;      
    }
    if (loanApplication.BorrowerEmploymentHireDate) {
      let hire: Date = new Date(loanApplication.BorrowerEmploymentHireDate);
      this.BorrowerHireDate = `${hire.getFullYear()}-${(hire.getMonth()+1).toString().padStart(2, "0")}-${hire.getDate().toString().padStart(2, "0")}`;      
    }
    if (loanApplication.BorrowerPhone) {
      let re = new RegExp("^\\d{10}$", "i");
      if (re.exec(loanApplication.BorrowerPhone)) {
        loanApplication.BorrowerPhone = loanApplication.BorrowerPhone.slice(0, 3) + "-" + loanApplication.BorrowerPhone.slice(3, 6) + "-" + loanApplication.BorrowerPhone.slice(-4);
      }
    }
    if (!loanApplication.BorrowerAddress) {
      loanApplication.BorrowerAddress = new Address();
    }
  }

  Save(): void {
    this.Saving = true;
    this.ErrorMessage = null;
    if (this.BorrowerBirthDate) {
      this.LoanApplication.BorrowerBirthDate = this.BorrowerBirthDate;
    }
    if (this.BorrowerHireDate) {
      this.LoanApplication.BorrowerEmploymentHireDate = this.BorrowerHireDate;
    }
    firstValueFrom(this.loanApplicationService.Create(this.LoanApplication))
    .then(app => {
      this.LoanApplication = app;
      this.InitializeLoanApplication(app);
    })
    .catch(err => {
      console.error(err);
      this.ErrorMessage = err.message || "Unexpected Error"
    })
    .finally(() => this.Saving = false)
    ;
  }

  GetEventValue(event: any) {
    console.log(event.target);
    return event.target.value;
  }
}
