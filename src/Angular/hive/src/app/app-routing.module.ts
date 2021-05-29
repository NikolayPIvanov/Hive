import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'auth', loadChildren: () => import('src/app/modules/auth/auth.module').then(m => m.AuthModule) },
  {
    path: 'home', loadChildren: () => import('src/app/modules/home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'account', loadChildren: () => import('src/app/modules/account/account.module').then(m => m.AccountModule),
  },
  { path: '', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
