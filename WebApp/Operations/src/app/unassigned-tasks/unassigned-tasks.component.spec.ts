import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnassignedTasksComponent } from './unassigned-tasks.component';

describe('UnassignedTasksComponent', () => {
  let component: UnassignedTasksComponent;
  let fixture: ComponentFixture<UnassignedTasksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnassignedTasksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnassignedTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
