import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskTypeGroupsComponent } from './task-type-groups.component';

describe('TaskTypeGroupsComponent', () => {
  let component: TaskTypeGroupsComponent;
  let fixture: ComponentFixture<TaskTypeGroupsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskTypeGroupsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskTypeGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
