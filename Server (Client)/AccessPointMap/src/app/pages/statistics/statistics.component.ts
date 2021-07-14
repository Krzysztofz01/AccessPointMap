import { AfterViewInit, Component } from '@angular/core';
import { AccessPointStatistics } from 'src/app/core/models/access-point-statistics.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import { ChartOptionGroup } from './chart-options-group.model';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements AfterViewInit {
  public securityChartOptions: ChartOptionGroup;
  public frequencyChartOptions: ChartOptionGroup;
  public brandChartOptions: ChartOptionGroup;
  public areaChartOptions: ChartOptionGroup;
  public accessPointCount: number;
  public accessPointInsecureCount: number;
  
  public initialized: boolean = false;

  public readonly colors: Array<any> = [{ backgroundColor: ['#000147', '#083271', '#10699f', '#1792c2', '#1cb5e0'] }];
  public readonly textColor: string = '#000000';
  public readonly ticksColor: string = '#000000';

  constructor(private accessPointService: AccessPointService) { }

  ngAfterViewInit(): void {
    this.accessPointService.getStatistics(1).subscribe((res) => {
      this.initializeCharts(res);
      this.initialized = true;
    },
    (error) => {
      console.error(error);
    });
  }

  private initializeCharts(chartData: AccessPointStatistics): void {
    //Single values
    this.accessPointCount = chartData.allRecords;
    this.accessPointInsecureCount = chartData.insecureRecords;

    //Security chart
    const securityLabels = new Array<string>();
    const securityValues = new Array<number>();
    chartData.topSecurityTypes.forEach(x => {
      securityLabels.push(x.item1);
      securityValues.push(x.item2);
    });

    this.securityChartOptions = {
      options: {
        responsive: true,
      },
      labels: securityLabels,
      data: securityValues,
      type: 'pie',
      legend: true,
      plugins: [],
      colors: this.colors
    };

    //Frequency chart
    const freqLabels = new Array<string>();
    const freqValues = new Array<number>();
    chartData.topFrequencies.forEach(x => {
      freqLabels.push(x.item1);
      freqValues.push(x.item2);
    });

    this.frequencyChartOptions = {
      options: {
        responsive: true,
      },
      labels: freqLabels,
      data: freqValues,
      type: 'pie',
      legend: true,
      plugins: [],
      colors: this.colors
    };

    //Area chart
    const areaLabels = new Array<string>();
    const areaValues = new Array<number>();
    chartData.topAreaAccessPoints.forEach(x => {
      areaLabels.push(x.ssid);
      areaValues.push(x.signalArea);
    });

    this.areaChartOptions = {
      dataArr: [
        { data: areaValues, label: 'Area' },
      ],
      labels: areaLabels,
      options: {
        responsive: true
      },
      colors: [{
        borderColor: '#000147',
        backgroundColor: 'rgba(28, 181, 224, .5)'
      }],
      legend: false,
      type: 'line',
      plugins: []
    };

    //Manufacturer chart
    const brandLabels = new Array<string>();
    const brandValues = new Array<number>();
    chartData.topBrands.forEach(x => {
      brandLabels.push(x.item1);
      brandValues.push(x.item2);
    });

    this.brandChartOptions = {
      dataArr: [
        { data: brandValues, label: 'Count' },
      ],
      labels: brandLabels,
      options: {
        responsive: true
      },
      colors: [{
        borderColor: '#000147',
        backgroundColor: 'rgba(28, 181, 224, .5)'
      }],
      legend: false,
      type: 'line',
      plugins: []
    };

      
  }
}
