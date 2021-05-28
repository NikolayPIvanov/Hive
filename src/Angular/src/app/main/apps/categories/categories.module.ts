import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRippleModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginatorModule } from '@angular/material/paginator';
import { NgxDnDModule } from '@swimlane/ngx-dnd';

import { FuseSharedModule } from '@fuse/shared.module';
import { FuseSidebarModule } from '@fuse/components';

import { CategoriesComponent } from './categories/categories.component';

const routes: Routes = [
  {
      path     : 'all',
      component: CategoriesComponent
  },
  {
      path      : '**',
      redirectTo: 'all'
  }
];

@NgModule({
  declarations: [
    CategoriesComponent
  ],
  imports: [
    RouterModule.forChild(routes),

    MatPaginatorModule,

    NgxDnDModule,

    FuseSharedModule,
    FuseSidebarModule
  ]
})
export class CategoriesModule { }
