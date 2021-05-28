import { DataSource } from '@angular/cdk/collections';
import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { fuseAnimations } from '@fuse/animations';
import { FuseUtils } from '@fuse/utils';
import { BehaviorSubject, fromEvent, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { FilesDataSource } from '../../dashboards/project/project.component';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
  animations   : fuseAnimations,
  encapsulation: ViewEncapsulation.None
})
export class CategoriesComponent implements OnInit {
  dataSource: FilesDataSource | null;
  displayedColumns = ['id', 'image', 'name', 'category', 'price', 'quantity', 'active'];

  @ViewChild(MatPaginator, {static: true})
  paginator: MatPaginator;

  @ViewChild(MatSort, {static: true})
  sort: MatSort;

  @ViewChild('filter', {static: true})
  filter: ElementRef;

    // Private
  private _unsubscribeAll: Subject<any>;
  
  constructor() {
    // Set the private defaults
    this._unsubscribeAll = new Subject();
  }

  /**
     * On init
     */
   ngOnInit(): void
   {
      //  this.dataSource = new FilesDataSource(this._ecommerceProductsService, this.paginator, this.sort);

       fromEvent(this.filter.nativeElement, 'keyup')
           .pipe(
               takeUntil(this._unsubscribeAll),
               debounceTime(150),
               distinctUntilChanged()
           )
           .subscribe(() => {
               if ( !this.dataSource )
               {
                   return;
               }

               this.dataSource.filter = this.filter.nativeElement.value;
           });
   }
}