import { Component, Input, OnInit } from '@angular/core';

import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.scss']
})
export class LineChartComponent implements OnInit {
  @Input() data!: number[]
  @Input() label!: string
  @Input() labels!: Label[]
  @Input() color: string = 'rgba(255,0,0,0.3)';
  @Input() type: ChartType = 'line';

  @Input() height: number = 50;
  @Input() width: number = 300;

  public lineChartData!: ChartDataSets[];
  public lineChartColors!: Color[] ;

  constructor() { }

  ngOnInit(): void {
    this.lineChartData = [{ data: this.data, label: this.label }];
    this.lineChartColors = [
      {
        borderColor: 'black',
        backgroundColor: this.color,
      },
    ]
  }
  
  public lineChartOptions: (ChartOptions) = {
    responsive: true,
  };
  public lineChartLegend = true;
  public lineChartPlugins = [];

}
