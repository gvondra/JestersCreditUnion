import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleRequestComponent } from './role-request.component';

describe('RoleRequestComponent', () => {
  let component: RoleRequestComponent;
  let fixture: ComponentFixture<RoleRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoleRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoleRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
