import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, PaginatedListOfGigOverviewDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gigs-list',
  templateUrl: './gigs-list.component.html',
  styleUrls: ['./gigs-list.component.scss']
})
export class GigsListComponent implements OnInit {
  private subject = new Subject();
  private categoryId: number | null = null;

  public paginatedList$: Observable<PaginatedListOfGigOverviewDto> | undefined;
  public categoryObservable$: Observable<CategoryDto> | undefined;

  constructor(
    private categoriesApiClient: CategoriesClient,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const param = this.activatedRoute.snapshot.paramMap.get('id');
    this.categoryId = param ? +param : null

    this.categoryObservable$ = this.categoriesApiClient.getCategoryById(this.categoryId!)

    this.paginatedList$ =
      this.categoriesApiClient.getCategoryGigs(this.categoryId!, undefined, undefined, undefined)
  }
}
