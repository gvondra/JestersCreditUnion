import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoanApplication } from '../models/loan-application';
import { LoanApplicationService } from '../services/loan-application.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-borrower-identification-code-upload',
  templateUrl: './borrower-identification-code-upload.component.html',
  styles: [
  ]
})
export class BorrowerIdentificationCodeUploadComponent implements OnInit {

  LoanApplication: LoanApplication | null = null;
  ErrorMessage: string | null = null;
  Saving: boolean = false;
  InputColumnClass: string = "col-md-4";
  FileToUpload: File | null = null;

  constructor(
    private activatedRoute: ActivatedRoute,
    private loanApplicationService: LoanApplicationService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.LoanApplication = null;
      this.ErrorMessage = null;
      this.FileToUpload = null;
      this.Saving = false;
      if (params["id"]) {
        this.LoadLoanApplication(params["id"]);
      }
    });
  }

  private LoadLoanApplication(id: string): void {
    firstValueFrom(this.loanApplicationService.Get(id))
    .then(app => this.LoanApplication = app)
    .catch(err => {
      console.error(err);
      this.ErrorMessage = err.message || "Unexpected Error"
    });          
  }

  Save() {
    if (this.FileToUpload && this.LoanApplication?.LoanApplicationId) {      
      this.Saving = true;
      this.ErrorMessage = null;
      const formData: FormData = new FormData();
      formData.append("file", this.FileToUpload);
      firstValueFrom(this.loanApplicationService.SaveIdentificationCard(this.LoanApplication.LoanApplicationId, formData))
      .then(res => console.log(res))
      .catch(err => {
        console.error(err);
        this.ErrorMessage = err.message || "Unexpected Error"
      })
      .finally(() => {
        this.Saving = false;        
      });
    }
  }

  UploadFile(file: any) {
    if (file.length === 1) {
      this.FileToUpload = file[0] as File;
    }    
  }

}
