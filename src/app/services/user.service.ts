import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { SignupUserResponse } from '../modules/interfaces/user/SignupUserResponse';
import { SignupUserRequest } from '../modules/interfaces/user/SignupUserRequest';
import { AuthRequest } from '../modules/interfaces/user/auth/AuthRequest';
import { AuthResponse } from '../modules/interfaces/user/auth/AuthResponse';

@Injectable({
  providedIn: 'root', //Indica que este serviço pode ser usado em qualquer classe
})
export class UserService {
  private API_URL = environment.API_URL; //de desenvolvimento

  constructor(
    private http: HttpClient,
    private cookie: CookieService
  ) {}

  //Criar um novo usuário ao clicar no botão 'criar conta':
  signupUser(requestDatas: SignupUserRequest): Observable<SignupUserResponse> {
    return this.http.post<SignupUserResponse>(`${this.API_URL}/user`, requestDatas);
  }

  //Autenticar um usuário criando um token JWT ao clicar no botão 'login':
  authUser(requestDatas: AuthRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/auth`, requestDatas);
  }

  //Route guard (rotas protegidas)
  isloggedIn(): boolean {
    //Verificar se o user possui um token nos cookies do browser
    const JWT_TOKEN = this.cookie.get('USER_INFO');
    return JWT_TOKEN ? true : false;
  }

}
