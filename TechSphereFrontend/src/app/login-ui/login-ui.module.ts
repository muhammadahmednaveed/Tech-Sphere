import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginUIRoutingModule } from './login-ui-routing.module';
import { LoginPageComponent } from './login-page/login-page.component';
import { RegistrationPageComponent } from './registration-page/registration-page.component';
import { ProfilePageComponent } from '../buyer-ui/profile-page/profile-page.component';
import { LoginUIComponent } from './login-ui.component';

@NgModule({
  declarations: [
    LoginPageComponent,
    RegistrationPageComponent,
    ProfilePageComponent,
    LoginUIComponent,
  ],
  imports: [
    CommonModule,
    LoginUIRoutingModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [LoginPageComponent],
})
export class LoginUIModule {}
