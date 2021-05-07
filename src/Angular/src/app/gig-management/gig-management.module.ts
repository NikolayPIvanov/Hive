import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { GigManagementRoutingModule } from './gig-management-routing.module';
import { GigsControlComponent } from './gigs-control/gigs-control.component';
import { GigsDetailsComponent } from './gigs-details/gigs-details.component';
import { GigsCreateComponent } from './gigs-create/gigs-create.component';
import { TagsListComponent } from './tags-list/tags-list.component';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { QuestionFormComponent } from './questions-list/question-form/question-form.component';
import { DynamicFormComponent } from './dynamic-form/dynamic-form.component';
import { DynFormsMaterialModule } from '@myndpm/dyn-forms/ui-material';
import { CategoriesModule } from './categories/categories.module';


@NgModule({
  declarations: [
    GigsControlComponent,
    GigsDetailsComponent,
    GigsCreateComponent,
    TagsListComponent,
    QuestionsListComponent,
    QuestionFormComponent,
    DynamicFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,

    CategoriesModule,
    
    GigManagementRoutingModule,
    DynFormsMaterialModule.forFeature()
  ]
})
export class GigManagementModule { }
