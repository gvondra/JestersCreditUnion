import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskTypeEventTypesComponent } from './task-type-event-types.component';

describe('TaskTypeEventTypesComponent', () => {
  let component: TaskTypeEventTypesComponent;
  let fixture: ComponentFixture<TaskTypeEventTypesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskTypeEventTypesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskTypeEventTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
