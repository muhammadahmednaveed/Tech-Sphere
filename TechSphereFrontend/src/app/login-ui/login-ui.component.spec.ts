import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginUiComponent } from './login-ui.component';

describe('LoginUiComponent', () => {
  let component: LoginUiComponent;
  let fixture: ComponentFixture<LoginUiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginUiComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginUiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
