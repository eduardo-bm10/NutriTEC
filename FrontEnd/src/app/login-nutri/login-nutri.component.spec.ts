import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginNUTRIComponent } from './login-nutri.component';

describe('LoginNUTRIComponent', () => {
  let component: LoginNUTRIComponent;
  let fixture: ComponentFixture<LoginNUTRIComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginNUTRIComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginNUTRIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
