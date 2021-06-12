import { NgModule } from '@angular/core';
import { LayoutComponent } from '../layout/layout/layout.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { RouterModule, Routes } from '@angular/router';
import { PlansControlComponent } from './components/plans-control/plans-control.component';


const routes: Routes = [
  {
      path: '',
      component: LayoutComponent,
      children: [
          {
              path: '',
              component: PlansControlComponent
          }
      ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlansRoutingModule { }
