import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isAuthenticated: Observable<boolean>;

  constructor(private authorizationService: AuthorizeService) { }

  ngOnInit(): void {
    this.isAuthenticated = this.authorizationService.isAuthenticated()
  }
}
