import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, mergeMap } from 'rxjs';
import { HttpClientUtilService } from '../http-client-util.service';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private httpClientUtil: HttpClientUtilService,
    private httpClient: HttpClient) { }  
  
  GetToken() : Observable<string> {
    return this.httpClientUtil.CreateUserTokenAuthHeader().pipe(
      mergeMap((headers: HttpHeaders) => {
        return this.httpClient.post(`${this.httpClientUtil.GetApiBaseAddress()}Token`, null, {headers: headers, responseType: 'text'});
      })
    );   
    
  }
  
}
