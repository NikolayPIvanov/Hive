import { Component, ContentChild, Input, OnInit, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.scss']
})
export class CardListComponent implements OnInit {
  @Input('items') items!: any[];
  @ContentChild(TemplateRef) layoutTemplate!: TemplateRef<any>;

  constructor() { }

  ngOnInit(): void {
  }

}
