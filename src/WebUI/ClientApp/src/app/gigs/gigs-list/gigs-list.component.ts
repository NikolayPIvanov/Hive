import { Component, Input, OnInit } from '@angular/core';
import { CategoriesClient, GigsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-gigs-list',
  templateUrl: './gigs-list.component.html',
  styleUrls: ['./gigs-list.component.css']
})
export class GigsListComponent implements OnInit {
  @Input() categoryId: number;
  pageNumber: number;
  pageSize: number;

  constructor(private categoryClient: CategoriesClient) { }

  ngOnInit(): void {
    this.categoryClient.getGigs(this.categoryId, this.pageNumber, this.pageSize)
  }

}
