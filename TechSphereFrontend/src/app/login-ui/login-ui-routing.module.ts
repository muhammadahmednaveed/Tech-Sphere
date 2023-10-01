import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from './login-page/login-page.component';
import { ProfilePageComponent } from '../buyer-ui/profile-page/profile-page.component';
import { RegistrationPageComponent } from './registration-page/registration-page.component';
import { LoginUIComponent } from './login-ui.component';
const routes: Routes = [
  {
    path: 'Account',
    component: LoginUIComponent,
    children: [
      { path: 'login', component: LoginPageComponent },
      { path: 'register', component: RegistrationPageComponent },
      { path: '', redirectTo: '/login', pathMatch: 'full' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],

  exports: [RouterModule],
})
export class LoginUIRoutingModule {}
