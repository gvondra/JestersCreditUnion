import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { mergeMap, Observable } from 'rxjs';
import { HttpClientUtilService } from '../http-client-util.service';
import { LoanApplication } from '../models/loan-application';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class LoanApplicationService {

  constructor(private httpClientUtil: HttpClientUtilService,
    private httpClient: HttpClient,
    private tokenService: TokenService) { }

  Create(loanApplication: LoanApplication): Observable<LoanApplication> {
    return this.httpClientUtil.CreateAuthHeader(this.tokenService).pipe(
      mergeMap((httpHeaders: HttpHeaders) => {
        return this.httpClient.post<LoanApplication>(`${this.httpClientUtil.GetApiBaseAddress()}LoanApplication`, loanApplication, {headers: httpHeaders})
      })
    );
  }
}
