import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '../core/core.module';
import { LayoutModule } from '../layout/layout.module';
import { CategoriesDashboardComponent } from './components/categories-dashboard/categories-dashboard.component';
import { CategoryRoutingModule } from './category-routing.module';
import { CategoryDetailsComponent } from './components/category-details/category-details.component';
import { CategoriesSearchComponent } from './components/categories-search/categories-search.component';
import { CategoriesCreateComponent } from './components/categories-create/categories-create.component';
import { CategoriesCreateModalComponent } from './components/categories-create-modal/categories-create-modal.component';



@NgModule({
  declarations: [
    CategoriesDashboardComponent,
    CategoryDetailsComponent,
    CategoriesSearchComponent,
    CategoriesCreateComponent,
    CategoriesCreateModalComponent
  ],
  imports: [
    CommonModule,

    LayoutModule,

    CategoryRoutingModule
  ]
})
export class CategoryModule { }
