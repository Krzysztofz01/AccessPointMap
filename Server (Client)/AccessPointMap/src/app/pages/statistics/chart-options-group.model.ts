import { ChartDataSets, ChartOptions, ChartType } from "chart.js";
import { Color, Label, SingleDataSet } from "ng2-charts";

export interface ChartOptionGroup {
    options: ChartOptions;
    labels: Array<Label>;
    data?: SingleDataSet;
    dataArr?: ChartDataSets[];
    type: ChartType;
    legend: boolean;
    plugins: Array<any>;
    colors: Color[];
}