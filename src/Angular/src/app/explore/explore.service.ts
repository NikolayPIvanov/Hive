import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ExploreService {
  private selectedCategory = new BehaviorSubject<number>(0);
  selectedCategory$ = this.selectedCategory.asObservable();

  constructor() { }

  changeSelectedCategory(id: number) {
    this.selectedCategory.next(id);
  }
}
