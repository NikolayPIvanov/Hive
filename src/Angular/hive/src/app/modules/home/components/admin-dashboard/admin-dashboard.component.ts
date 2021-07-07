import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoriesClient, CategoriesType, PaginatedListOfCategoryDto } from 'src/app/clients/gigs-client';
import { FileResponse, PaginatedListOfUserProfileDto, ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  public categories$: Observable<PaginatedListOfCategoryDto> | undefined;
  public users$: Observable<PaginatedListOfUserProfileDto> | undefined;

  constructor(
    private categoriesClient: CategoriesClient,
    private profilesClient: ProfileClient,
    private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.categories$ = this.categoriesClient.getCategories(1, 3, CategoriesType.All, null)
    this.users$ = this.profilesClient.getProfiles(1, 3);
  }

  downloadFunc(user: UserProfileDto) {
    return this.profilesClient.getAvatar(user.id!);
  }

  public lineChartData: ChartDataSets[] = [
    { data: [65, 59, 80, 81, 56, 55, 40, 45,60,80], label: 'Users' },
  ];
  public lineChartLabels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October'];
  public lineChartOptions: (ChartOptions) = {
    responsive: true,
  };
  public lineChartColors: Color[] = [
    {
      borderColor: 'black',
      backgroundColor: 'rgba(255,0,0,0.3)',
    },
  ];
  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';
  public lineChartPlugins = [];


}
