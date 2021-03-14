import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GigsListComponent } from './gigs-list/gigs-list.component';
import { GigCardComponent } from './gig-card/gig-card.component';
import { MatCardModule } from '@angular/material/card';

@NgModule({
  declarations: [GigsListComponent, GigCardComponent],
  imports: [
    CommonModule,
    MatCardModule
  ]
})
export class GigsModule { }
