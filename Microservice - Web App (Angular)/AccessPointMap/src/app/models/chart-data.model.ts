import { ChartType } from "chart.js";

export interface ChartData {
    type: ChartType;
    labels: Array<string>;
    content: Array<any>;
    colors: Array<string>;
}