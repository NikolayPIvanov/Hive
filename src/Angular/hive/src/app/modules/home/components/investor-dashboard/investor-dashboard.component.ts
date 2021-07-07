import { Component, OnInit } from '@angular/core';
import { Label } from 'ng2-charts';
import { Observable } from 'rxjs';
import { InvestmentDto, InvestorsClient } from 'src/app/clients/investing-client';


@Component({
  selector: 'app-investor-dashboard',
  templateUrl: './investor-dashboard.component.html',
  styleUrls: ['./investor-dashboard.component.scss']
})
export class InvestorDashboardComponent implements OnInit {
  public investments$: Observable<InvestmentDto[]> | undefined

  constructor(private investorsClient: InvestorsClient) { }

  ngOnInit(): void {
    // this.investments$ = this.investorsClient.getInvestments()
  }

  public label: string = 'Revenue'
  public data: number[] = [0, 200, 350, 350, 567, 980, 1700, 2300, 2800, 3942, 4900, 5600]
  public labels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  public color: string = 'rgba(255,0,0,0.3)'
  
}
