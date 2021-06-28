import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoriesClient, CategoriesType, PaginatedListOfCategoryDto } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  public categories$: Observable<PaginatedListOfCategoryDto>  | undefined;

  constructor(private categoriesClient: CategoriesClient) { }

  ngOnInit(): void {
    this.categories$ = this.categoriesClient.getCategories(1, 8, CategoriesType.All, null)
  }

}
