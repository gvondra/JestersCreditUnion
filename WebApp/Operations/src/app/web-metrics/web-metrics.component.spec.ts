import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WebMetricsComponent } from './web-metrics.component';

describe('WebMetricsComponent', () => {
  let component: WebMetricsComponent;
  let fixture: ComponentFixture<WebMetricsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WebMetricsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WebMetricsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
