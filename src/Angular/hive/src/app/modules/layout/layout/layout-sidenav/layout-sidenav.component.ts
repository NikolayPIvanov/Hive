import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-layout-sidenav',
  templateUrl: './layout-sidenav.component.html',
  styleUrls: ['./layout-sidenav.component.scss']
})
export class LayoutSidenavComponent implements OnInit {
  @Input('mobileQuery') mobileQuery!: MediaQueryList;
  
  constructor() { }

  ngOnInit(): void {
  }

}
