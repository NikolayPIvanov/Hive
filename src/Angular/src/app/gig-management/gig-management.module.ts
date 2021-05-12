import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { GigManagementRoutingModule } from './gig-management-routing.module';
import { GigsControlComponent } from './gigs/gigs-control/gigs-control.component';
import { GigsDetailsComponent } from './gigs/gigs-details/gigs-details.component';
import { GigsCreateComponent } from './gigs-create/gigs-create.component';
import { TagsListComponent } from './tags-list/tags-list.component';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { QuestionFormComponent } from './questions-list/question-form/question-form.component';
import { DynFormsMaterialModule } from '@myndpm/dyn-forms/ui-material';
import { CategoriesModule } from './categories/categories.module';
import { GigsModule } from './gigs/gigs.module';

@NgModule({
  declarations: [
    GigsDetailsComponent,
    GigsCreateComponent,
    TagsListComponent,
    QuestionsListComponent,
    QuestionFormComponent,
    
  ],
  imports: [
    CommonModule,
    SharedModule,

    CategoriesModule,
    GigsModule,
    
    GigManagementRoutingModule,
    DynFormsMaterialModule.forFeature()
  ]
})
export class GigManagementModule { }
