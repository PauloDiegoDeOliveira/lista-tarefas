import { CommonModule } from '@angular/common';
import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from '../../../services/user.service';
import { AuthRequest } from '../../interfaces/user/auth/AuthRequest';
import { SignupUserRequest } from '../../interfaces/user/SignupUserRequest';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  imports: [CommonModule, ReactiveFormsModule],
})
export class HomeComponent implements OnDestroy {
  private destroy$ = new Subject<void>();
  loginCard = true;
  loginForm: FormGroup;
  signupForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private cookieService: CookieService, //Biblioteca externa ngx-cookie-service
    private messageService: MessageService,
    private router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      //Para login em conta existente
      email: ['', Validators.required],
      password: ['', Validators.required],
    });

    this.signupForm = this.formBuilder.group({
      //Para criar uma conta
      name: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  //Para login em conta existente
  onSubmitLoginForm(): void {
    if (this.loginForm.value && this.loginForm.valid) {
      this.userService
        .authUser(this.loginForm.value as AuthRequest)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            if (response) {
              //Após autenticar o login, guarda o token JWT recebido na response em um cookie:
              this.cookieService.set('USER_INFO', response?.token); //Guardar o token nos cookies
              this.loginForm.reset();
              this.router.navigate(['/dashboard']);

              //Exibe um popup com mensagem de login feito com sucesso ou falha:
              this.messageService.add({
                severity: 'success',
                summary: 'Sucesso',
                detail: `Bem-vindo de volta, ${response?.name}!`,
                life: 3000,
              });
            }
          },
          error: (err) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Erro',
              detail: `Erro ao fazer login!`,
              life: 3000,
            });
            console.log(err);
          },
        });
    }
  }

  //Para criar uma conta
  onSubmitSignupForm(): void {
    //Value é se o formulário possui algum valor e Valid é caso tenha preenchido os campos
    if (this.signupForm.value && this.signupForm.valid) {
      this.userService
        .signupUser(this.signupForm.value as SignupUserRequest)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            if (response) {
              this.signupForm.reset(); //Para limpar todos os campos do form após o envio
              this.loginCard = true; //Para redirecionar ao form de login após criar o user
              this.messageService.add({
                severity: 'success',
                summary: 'Sucesso',
                detail: `Usuário criado com sucesso!`,
                life: 3000,
              });
            }
          },
          error: (err) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Erro',
              detail: `Erro ao criar usuário!`,
              life: 3000,
            });
            console.log(err);
          },
        });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
