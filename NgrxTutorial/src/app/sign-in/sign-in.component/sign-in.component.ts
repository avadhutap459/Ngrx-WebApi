import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { combineLatest } from 'rxjs';
import { selectError, selectLoading } from '../store/auth.reducer';
import { authActions } from '../store/auth.action';

@Component({
  selector: 'app-sign-in.component',
  standalone: false,
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.css',
})
export class SignInComponent {
  loginForm: FormGroup;
  hidePassword = true;
  isLoading = false;

  private store = inject(Store);

  data$ = combineLatest({
    isSubmitting: this.store.select(selectError),
    backendError: this.store.select(selectLoading)
  })

  constructor(private fb: FormBuilder,
    private _userSvc: UserService,
    private _router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  submit() {
    debugger
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    const loginData = this.loginForm.value;


    this.store.dispatch(authActions.login({ emailId : loginData.email, password : loginData.password }));

  }
}
