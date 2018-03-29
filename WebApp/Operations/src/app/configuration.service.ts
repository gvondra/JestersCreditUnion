import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
@Injectable()
export class ConfigurationService {
  private configurationUrl: string;
  constructor(private http: Http) {
    this.configurationUrl = environment.configurationServiceUrl + "/Configuration";
   }

   getLookupServiceUrl(): Promise<string> {
     return Promise.resolve("lookup service url");
   }
}
