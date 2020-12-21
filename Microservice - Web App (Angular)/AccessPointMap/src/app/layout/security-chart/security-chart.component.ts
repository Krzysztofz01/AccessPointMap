import { Component, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { ChartPreparationService } from 'src/app/services/chart-preparation.service';

const CACHE_KEY = "VECTOR_LAYER_ACCESSPOINTS";

@Component({
  selector: 'security-chart',
  templateUrl: './security-chart.component.html',
  styleUrls: ['./security-chart.component.css']
})
export class SecurityChartComponent implements OnInit {
  public chartLabels: Label[];
  public chartDisplayType: ChartType;
  public chartLegend: boolean;
  public chartOptions: ChartOptions;
  public chartData: ChartDataSets[];

  constructor(private cacheService: CacheManagerService, private chartPreparationService: ChartPreparationService) { }

  ngOnInit(): void {
    const accesspoints = this.cacheService.load(CACHE_KEY);
    const preparedChartData = this.chartPreparationService.prepareSecurityChart(accesspoints);
    
    this.chartLabels = preparedChartData.labels;
    this.chartData = [
      { data: preparedChartData.content }
    ];
    this.chartDisplayType = 'bar';
    this.chartLegend = false;
    this.chartOptions = {
      responsive: true
    }
  }
}
