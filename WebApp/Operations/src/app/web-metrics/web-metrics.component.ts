import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { WebMetricService } from '../web-metric.service';
import { WebMetric } from '../web-metric';

@Component({
  selector: 'app-web-metrics',
  templateUrl: './web-metrics.component.html',
  styleUrls: ['./web-metrics.component.css'],
  providers: [WebMetricService]
})
export class WebMetricsComponent implements OnInit {

  Page: number = null;
  UntilPage: Date = null;
  UntilMessage: string = null;
  Until: string = null;  
  WebMetrics: Array<WebMetric> = null;
  SpinnerHidden: boolean = true;
  PreviousHidden: boolean = true;

  constructor(private route: ActivatedRoute, private router: Router, private webMetricService: WebMetricService) { }

  Load() {
    this.UntilMessage = null;
    if (Date.parse(this.Until)) {
      this.router.navigate(["/webmetrics"], {queryParams: { until: new Date(Date.parse(this.Until)).toISOString(), p: 0 }});
    }
    else {
      this.UntilMessage = "Invalid Date";
    }
  } 

  GetUntilISOString() {
    return this.UntilPage.toISOString()
  }

  RoundDuration(d: number) {
    if (d) {
      d = Math.round(d * 100000) / 100000
    }
    return d;
  }

  FormatTimestamp(ts: string) {
    if (ts && Date.parse(ts)) {
      let d = new Date(Date.parse(ts));
      ts = d.toLocaleString('en-us', {hour12: false});
    }
    return ts;
  }

  ngOnInit() {
    this.route.queryParams
    .switchMap((params: ParamMap) => {
      this.SpinnerHidden = false;
      this.Page = parseInt(params['p'] || "0");
      this.PreviousHidden = (this.Page == 0);
      if (params['until'] && Date.parse(params['until'])) {
        let d: Date = new Date(Date.parse(params['until']));
        this.UntilPage = d;
        this.Until = d.toLocaleString('en-us', {hour12: false});
        return this.webMetricService.getWebMetricsByMaxCreateTimestamp(d, this.Page);
      }
      else {
        let d: Date = new Date();
        this.Until = new Date(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes(), d.getSeconds(), d.getMilliseconds()).toLocaleString('en-us', {hour12: false});
        this.UntilPage = null;
        return Promise.resolve(null);
      }
    })
    .subscribe(webMetrics => {
      this.WebMetrics = webMetrics;
      this.SpinnerHidden = true;
    });
  }

}
