import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { LookupItem } from './lookup-item';
import { LOOKUP_ITEMS } from './mock-lookup-items';
@Injectable()
export class LookupService {

  constructor(private http: Http) { }

  getActiveLookup(group: string, selected: string): Promise<LookupItem[]> {    
    return Promise.resolve(LOOKUP_ITEMS[group]);
  }
}
