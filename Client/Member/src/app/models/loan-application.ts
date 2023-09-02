import { Address } from './address';
import { LoanApplicationComment } from './loan-application-comment';
export class LoanApplication {
    StatusDescription: string | null = null;
    LoanApplicationId: string | null = null;
    BorrowerName: string = "";
    BorrowerBirthDate: string | null = null;
    BorrowerAddress: Address | null = null;
    BorrowerEmailAddress: string = "";
    BorrowerPhone: string = "";
    BorrowerEmployerName: string = "";
    BorrowerEmploymentHireDate: string | null = null;
    BorrowerIncome: number | null = null;
    CoBorrowerName: string = "";
    CoBorrowerBirthDate: string | null = null;
    CoBorrowerAddress: Address | null = null;
    CoBorrowerEmailAddress: string = "";
    CoBorrowerPhone: string = "";
    CoBorrowerEmployerName: string = "";
    CoBorrowerEmploymentHireDate: string | null = null;
    CoBorrowerIncome: number | null = null;
    Amount: number | null = null;
    Purpose: string = "";
    MortgagePayment: number | null = null;
    RentPayment: number | null = null;
    CreateTimestamp: string | null = null;
    UpdateTimestamp: string | null = null;
    Comments: LoanApplicationComment[] | null = [];
}
