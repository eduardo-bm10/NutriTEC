import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginADMINComponent } from './login-admin.component';

describe('LoginADMINComponent', () => {
  let component: LoginADMINComponent;
  let fixture: ComponentFixture<LoginADMINComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginADMINComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginADMINComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
