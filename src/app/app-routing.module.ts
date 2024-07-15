import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './modules/home/home.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  // {
  //   path: 'dashboard',
  //   loadChildren: () =>
  //     import('./modules/dashboard/dashboard.module').then(
  //       (m) => m.DashboardModule
  //     ),
  //   canActivate: [AuthGuard], //Route guard - Para confirmar uma rota privada, liberada apenas para users logados
  // },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    preloadingStrategy: PreloadAllModules, //Cache dos módulos da aplicação para melhorar a performance
  })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
