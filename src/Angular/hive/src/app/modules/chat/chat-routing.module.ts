import { NgModule } from '@angular/core';
import { LayoutComponent } from '../layout/layout/layout.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { RouterModule, Routes } from '@angular/router';
import { ChatComponent } from './components/chat/chat.component';


const routes: Routes = [
  {
      path: '',
      component: LayoutComponent,
      children: [
          {
              path: '',
              component: ChatComponent
          }
      ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
