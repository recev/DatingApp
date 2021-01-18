import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RolesEditModalComponent } from './roles-edit-modal.component';

describe('RolesEditModalComponent', () => {
  let component: RolesEditModalComponent;
  let fixture: ComponentFixture<RolesEditModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RolesEditModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RolesEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
