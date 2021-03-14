import { Component, Input, OnInit } from '@angular/core';
import { GigDto, GigsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-gig-card',
  templateUrl: './gig-card.component.html',
  styleUrls: ['./gig-card.component.css']
})
export class GigCardComponent implements OnInit {
  @Input() gig: GigDto;

  constructor() { }

  ngOnInit(): void {
  }

}
