import { Component } from '@angular/core';
import { Router, Route } from '@angular/router';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'hive';

  constructor() { }
  
  ngOnInit() {
  }
}
