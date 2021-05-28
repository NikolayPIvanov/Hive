import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GigsControlPanelComponent } from './components/gigs-control-panel/gigs-control-panel.component';
import { GigsListComponent } from './components/gigs-list/gigs-list.component';
import { GigsRoutingModule } from './gigs-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { GigsSingleCardComponent } from './components/gigs-single-card/gigs-single-card.component';
import { AvatarModule } from 'ngx-avatar';
import { GigsListFilterComponent } from './components/gigs-list-filter/gigs-list-filter.component';

@NgModule({
  declarations: [
    GigsControlPanelComponent,
    GigsListComponent,
    GigsSingleCardComponent,
    GigsListFilterComponent
  ],
  imports: [
    CommonModule,

    SharedModule,

    AvatarModule,
    
    GigsRoutingModule
  ],
  exports: [
    GigsSingleCardComponent
  ]
})
export class GigsModule { }
