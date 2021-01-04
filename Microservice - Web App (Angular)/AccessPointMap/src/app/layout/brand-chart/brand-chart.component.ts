import { Component, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Colors, Label } from 'ng2-charts';
import { ChartData } from 'src/app/models/chart-data.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { ChartPreparationService } from 'src/app/services/chart-preparation.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'brand-chart',
  templateUrl: './brand-chart.component.html',
  styleUrls: ['./brand-chart.component.css']
})
export class BrandChartComponent implements OnInit {
  public chartLabels: Label[];
  public chartDisplayType: ChartType;
  public chartLegend: boolean;
  public chartOptions: ChartOptions;
  public chartColors: Colors[];
  public chartData: ChartDataSets[];
  public chartReady: boolean;

  constructor(private cacheService: CacheManagerService, private chartPreparationService: ChartPreparationService, private accesspointDataService: AccesspointDataService) { }

  ngOnInit(): void {
    this.chartReady = false;
    this.initializeData();
  }

  private chartSetup(preparedChartData: ChartData): void {
    this.chartLabels = preparedChartData.labels;
    this.chartData = [
      { data: preparedChartData.content }
    ];
    this.chartDisplayType = 'bar';
    this.chartLegend = false;
    this.chartOptions = {
      responsive: true,
      scales: {
        yAxes: [{
          ticks: {
            fontColor: preparedChartData.colors[0]
          }
        }],
        xAxes: [{
          ticks: {
            fontColor: preparedChartData.colors[0]
          }
        }]
      }
    };
    this.chartColors = [
      {
        backgroundColor: preparedChartData.colors[0]
      }
    ];
    this.chartReady = true;
  }

  private initializeData(): void {
    let preparedChartData: ChartData = this.cacheService.load(environment.CACHE_CHART_BRANDS);
    if(preparedChartData == null) {
      const accesspoints = this.cacheService.load(environment.CACHE_ACCESSPOINTS);
      if(accesspoints == null) {
        this.accesspointDataService.getAllAccessPoints().toPromise()
          .then(x => {
            this.chartPreparationService.prepareCharts(x);
            preparedChartData = this.cacheService.load(environment.CACHE_CHART_BRANDS);
            this.chartSetup(preparedChartData);
          })
          .catch(error => {
            console.log(error);
          });
      } else {
        this.chartPreparationService.prepareCharts(accesspoints);
        preparedChartData = this.cacheService.load(environment.CACHE_CHART_BRANDS);
        this.chartSetup(preparedChartData);
      }
    } else {
      this.chartSetup(preparedChartData);
    }
  }
}
