import { Component, Input, OnInit } from '@angular/core';
import { LoanApplication } from '../models/loan-application';
import { LoanApplicationComment } from '../models/loan-application-comment';
import { LoanApplicationService } from '../services/loan-application.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-loan-application-view',
  templateUrl: './loan-application-view.component.html',
  styles: [
  ]
})
export class LoanApplicationViewComponent implements OnInit {

  @Input() LoanApplication: LoanApplication | null = null;  
  LabelColumnClass: string = "col-md-3";
  InputColumnClass: string = "col-md-5";
  CommentText: string = "";

  get BorrowerBirthDate(): string | null {
    let result: string = "";
    if (this.LoanApplication) {
      let birth: Date = new Date(this.LoanApplication.BorrowerBirthDate as string);
      result = `${birth.getFullYear()}-${(birth.getMonth()+1).toString().padStart(2, "0")}-${birth.getDate().toString().padStart(2, "0")}`;      
    }
    return result;
  }

  get BorrowerPhone(): string | null {
    let result: string = this.LoanApplication?.BorrowerPhone || "";
    let re = new RegExp("^\\d{10}$", "i");
    if (re.exec(result)) {
      result = result.slice(0, 3) + "-" + result.slice(3, 6) + "-" + result.slice(-4);  
    }
    return result;
  }

  get BorrowerHireDate(): string | null {
    let result: string = "";
    if (this.LoanApplication) {
      let birth: Date = new Date(this.LoanApplication.BorrowerEmploymentHireDate as string);
      result = `${birth.getFullYear()}-${(birth.getMonth()+1).toString().padStart(2, "0")}-${birth.getDate().toString().padStart(2, "0")}`;      
    }
    return result;
  }

  get CoBorrowerBirthDate(): string | null {
    let result: string = "";
    if (this.LoanApplication) {
      let birth: Date = new Date(this.LoanApplication.CoBorrowerBirthDate as string);
      result = `${birth.getFullYear()}-${(birth.getMonth()+1).toString().padStart(2, "0")}-${birth.getDate().toString().padStart(2, "0")}`;      
    }
    return result;
  }

  get CoBorrowerPhone(): string | null {
    let result: string = this.LoanApplication?.CoBorrowerPhone || "";
    let re = new RegExp("^\\d{10}$", "i");
    if (re.exec(result)) {
      result = result.slice(0, 3) + "-" + result.slice(3, 6) + "-" + result.slice(-4);  
    }
    return result;
  }

  get CoBorrowerHireDate(): string | null {
    let result: string = "";
    if (this.LoanApplication) {
      let birth: Date = new Date(this.LoanApplication.CoBorrowerEmploymentHireDate as string);
      result = `${birth.getFullYear()}-${(birth.getMonth()+1).toString().padStart(2, "0")}-${birth.getDate().toString().padStart(2, "0")}`;      
    }
    return result;
  }

  get IncludeCoBorrower(): boolean {
    let result: boolean = false;
    if (this.LoanApplication?.CoBorrowerName || this.LoanApplication?.CoBorrowerBirthDate || this.LoanApplication?.CoBorrowerEmailAddress) {
      result = true;
    }
    return result;
  }

  constructor(private loanApplicationService: LoanApplicationService) { }

  ngOnInit(): void {
  }

  ToLocalDateString(value: any) : string {
    let result : string = "";
    if (value) {
      const date: Date = new Date(value);
      result = date.toLocaleDateString();
    }
    return result;
  }

  UpdateCommentText(event: any): void {
    this.CommentText = event.target.value || "";
  }

  AddComment() : void {
    if (this.LoanApplication) {
      const comment: LoanApplicationComment = new LoanApplicationComment();
      comment.Text = this.CommentText;
      comment.CreateTimestamp = (new Date()).toISOString();
      this.CommentText = "";
      if (!this.LoanApplication.Comments) {
        this.LoanApplication.Comments = [];
      }
      console.log(comment);
      if (comment.Text !== "" && this.LoanApplication?.LoanApplicationId) {      
        firstValueFrom(this.loanApplicationService.AppendComment(this.LoanApplication.LoanApplicationId, comment))
        .then(res => this.LoanApplication?.Comments?.splice(0, 0, comment))
        .catch(err => console.error(err));
      }
    }
  }

}
