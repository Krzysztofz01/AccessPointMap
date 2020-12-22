import { Component, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { ChartData } from 'src/app/models/chart-data.model';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { ChartPreparationService } from 'src/app/services/chart-preparation.service';

@Component({
  selector: 'frequency-chart',
  templateUrl: './frequency-chart.component.html',
  styleUrls: ['./frequency-chart.component.css']
})
export class FrequencyChartComponent implements OnInit {
  public chartLabels: Label[];
  public chartDisplayType: ChartType;
  public chartLegend: boolean;
  public chartOptions: ChartOptions;
  public chartData: ChartDataSets[];

  constructor(private cacheService: CacheManagerService, private chartPreparationService: ChartPreparationService) { }

  ngOnInit(): void {
    this.chartPreparationService.prepareCharts(this.cacheService.load('VECTOR_LAYER_ACCESSPOINTS'));
    const preparedChartData: ChartData = this.cacheService.load('CHART_FREQUENCY');
    
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
