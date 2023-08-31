import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { mergeMap, Observable } from 'rxjs';
import { HttpClientUtilService } from '../http-client-util.service';
import { LoanApplication } from '../models/loan-application';
import { TokenService } from './token.service';
import { LoanApplicationComment } from '../models/loan-application-comment';

@Injectable({
  providedIn: 'root'
})
export class LoanApplicationService {

  constructor(private httpClientUtil: HttpClientUtilService,
    private httpClient: HttpClient,
    private tokenService: TokenService) { }

  Search(byRequestor: boolean = true) : Observable<LoanApplication[]> {
    let params: HttpParams = new HttpParams();
    if (byRequestor) {
      params = params.append("byRequestor", "true")
    }
    return this.httpClientUtil.CreateAuthHeader(this.tokenService).pipe(
      mergeMap((httpHeaders: HttpHeaders) => {
        return this.httpClient.get<LoanApplication[]>(`${this.httpClientUtil.GetApiBaseAddress()}LoanApplication`, {headers: httpHeaders, params: params});
      })
    );
  }

  Get(id: string) : Observable<LoanApplication> {
    return this.httpClientUtil.CreateAuthHeader(this.tokenService).pipe(
      mergeMap((httpHeaders: HttpHeaders) => {
        return this.httpClient.get<LoanApplication>(`${this.httpClientUtil.GetApiBaseAddress()}LoanApplication/${id}`, { headers: httpHeaders});
      })
    );
  }

  Create(loanApplication: LoanApplication): Observable<LoanApplication> {
    return this.httpClientUtil.CreateAuthHeader(this.tokenService).pipe(
      mergeMap((httpHeaders: HttpHeaders) => {
        return this.httpClient.post<LoanApplication>(`${this.httpClientUtil.GetApiBaseAddress()}LoanApplication`, loanApplication, {headers: httpHeaders});
      })
    );
  }

  AppendComment(id: string, comment: LoanApplicationComment, isPublic: boolean = true) : Observable<any> {
    let params: HttpParams = new HttpParams();
    if (isPublic) {
      params = params.append("isPublic", "true")
    }
    return this.httpClientUtil.CreateAuthHeader(this.tokenService).pipe(
      mergeMap((httpHeaders: HttpHeaders) => {
        return this.httpClient.post(`${this.httpClientUtil.GetApiBaseAddress()}LoanApplication/${id}/Comment`, comment, {headers: httpHeaders, params: params})
      })
    );
  }

  SaveIdentificationCard(id: string, formData: FormData) : Observable<any> {
    return this.httpClientUtil.CreateAuthHeader(this.tokenService).pipe(
      mergeMap((httpHeaders: HttpHeaders) => {
        return this.httpClient.post(`${this.httpClientUtil.GetApiBaseAddress()}LoanApplication/${id}/BorrowerIdentificationCard`, formData, { headers: httpHeaders });
      })      
    );
  }
}
