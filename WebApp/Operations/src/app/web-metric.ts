import { WebMetricAttribute } from './web-metric-attribute';
export class WebMetric {
    WebMetricId: number
    Url: string
    Method: string
    CreateTimestamp: Date
    Duration: number
    Status: string
    Controller: string
    Attributes: Array<WebMetricAttribute>
}
