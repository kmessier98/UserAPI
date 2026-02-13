import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { catchError, EMPTY, exhaustMap, finalize, Subject, Subscription, tap } from 'rxjs';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/authService';
import { passwordMatchValidator } from '../../Utils/function';

interface Role {
  value: string;
  viewValue: string;
}

@Component({
  selector: 'app-register',
  imports: [FormsModule, ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './register.html',
  styleUrls: ['./register.scss'],
})
export class Register implements OnInit, OnDestroy {
  // Propriétés publique
  isSubmitting = false;
  profileForm!: FormGroup;
  roles: Role[] = [];
  subscriptions: Subscription[] = [];

  // Propriétés privée
  private submit$ = new Subject<void>();

  // Constructeur
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {}

  // Méthodes du cycle de vie
  ngOnInit(): void {
    this.loadRoles();
    this.initializeForm();
    this.listenToSubmit();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => subscription.unsubscribe());
    this.submit$.complete();
  }

  // Getters
  get isFormInvalid(): boolean {
    return this.profileForm.invalid;
  }

  get userName() {
    return this.profileForm.get('userName');
  }

  get password() {
    return this.profileForm.get('password');
  }

  get email() {
    return this.profileForm.get('email');
  }

  get firstName() {
    return this.profileForm.get('FirstName');
  }

  get lastName() {
    return this.profileForm.get('LastName');
  }

  get dateOfBirth() {
    return this.profileForm.get('dateOfBirth');
  }

  get role() {
    return this.profileForm.get('role');
  }

  get confirmPassword() {
    return this.profileForm.get('confirmPassword');
  }

  // Méthodes publiques
  onSubmit(): void {
    if (!this.isFormInvalid) {
      this.submit$.next();
    } else {
      console.log('Form is invalid');
    }
  }

  // Méthodes privées
  private loadRoles(): void {
    this.roles = [
      { value: 'user', viewValue: 'Utilisateur' },
      { value: 'admin', viewValue: 'Administrateur' },
    ];
  }

  private initializeForm(): void {
    this.profileForm = this.fb.group(
      {
        username: ['', [Validators.required, Validators.minLength(3)]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
        role: [this.roles[0].value, Validators.required],
        email: ['', [Validators.required, Validators.email]],
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
      },
      { validators: passwordMatchValidator },
    );
  }
  private listenToSubmit(): void {
    this.submit$
      .pipe(
        exhaustMap(() => {
          this.isSubmitting = true;
          const { confirmPassword, ...formData } = this.profileForm.value;

          return this.authService.register(formData).pipe(
            //Succès uniquement
            tap(() => {
              this.router.navigate(['/login']);
            }),

            //Gestion des erreurs
            catchError((error) => {
              if (error.status === 409) {
                this.profileForm.setErrors({ serverError: "Nom d'utilisateur déjà pris." });
                return EMPTY;
              }

              console.error('Registration failed:', error);
              return EMPTY;
            }),

            finalize(() => {
              this.isSubmitting = false;
            }),
          );
        }),
      )
      .subscribe();
  }
}
