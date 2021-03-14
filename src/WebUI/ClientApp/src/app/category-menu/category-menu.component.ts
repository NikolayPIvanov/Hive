import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { CategoriesClient, CategoryDto } from '../web-api-client';

@Component({
  selector: 'app-category-menu',
  templateUrl: './category-menu.component.html',
  styleUrls: ['./category-menu.component.scss']
})
export class CategoryMenuComponent implements OnInit {
  MENU_LIMIT = 8;
  ONLY_PARENTS = true;

  $categories: Observable<CategoryDto[]>
  isAuthenticated: Observable<boolean>;

  constructor(private categoriesClient: CategoriesClient, private authorizationService: AuthorizeService) { }

  ngOnInit(): void {
    this.$categories = this.getParentCategories();
    this.isAuthenticated = this.authorizationService.isAuthenticated()
  }

  getParentCategories() {
    return this.categoriesClient.getAll(this.MENU_LIMIT, this.ONLY_PARENTS)
  }

}
