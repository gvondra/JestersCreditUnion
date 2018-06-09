import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { WebMetric } from './web-metric';

@Injectable()
export class WebMetricService {

  constructor(private http: Http) { }

  getWebMetricsByMaxCreateTimestamp(until: Date, page: number): Promise<Array<WebMetric>> {
    let query = new URLSearchParams();
    query.append("until", until.toISOString());
    query.append("page", String(page));
    return this.http.get(environment.baseUrl + "WebMetrics", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`}),
      params: query
    })
    .toPromise()
    .then(response => response.json());
  }
}
