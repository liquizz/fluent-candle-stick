import { Component, Input, OnInit, ElementRef, ViewChild, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { createChart, IChartApi, CandlestickSeries } from 'lightweight-charts';
import { CandleStick } from '../../models/candlestick.model';

@Component({
  selector: 'app-candlestick-chart',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="chart-container" #chartContainer></div>
  `,
  styles: [`
    .chart-container {
      width: 100%;
      height: 400px;
    }
  `]
})
export class CandlestickChartComponent implements OnInit, OnDestroy {
  @ViewChild('chartContainer') chartContainer!: ElementRef;
  @Input() data: CandleStick[] = [];

  private chart?: IChartApi;
  private series?: any;

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.initChart();
  }

  ngOnDestroy(): void {
    if (this.chart) {
      this.chart.remove();
    }
  }

  private initChart(): void {
    const chartOptions = {
      layout: {
        background: { color: '#ffffff' },
        textColor: '#333',
      },
      width: this.chartContainer.nativeElement.clientWidth,
      height: 400,
      grid: {
        vertLines: { color: '#f0f0f0' },
        horzLines: { color: '#f0f0f0' },
      },
      timeScale: {
        timeVisible: true,
        secondsVisible: false,
      },
    };

    this.chart = createChart(this.chartContainer.nativeElement, chartOptions);

    this.series = this.chart.addSeries(CandlestickSeries, {
      upColor: '#26a69a',
      downColor: '#ef5350',
      borderVisible: false,
      wickUpColor: '#26a69a',
      wickDownColor: '#ef5350',
    });

    this.updateChartData();

    // Handle window resize
    window.addEventListener('resize', this.handleResize);
  }

  private updateChartData(): void {
    if (!this.series || !this.data || this.data.length === 0) return;

    console.log('Updating chart data with:', this.data);

    const chartData = this.data.map(candle => {
      // Convert string time to Unix timestamp (seconds)
      const timestamp = Math.floor(new Date(candle.time).getTime() / 1000);

      return {
        time: timestamp,
        open: candle.open,
        high: candle.high,
        low: candle.low,
        close: candle.close,
      };
    });

    console.log('Formatted chart data:', chartData);

    this.series.setData(chartData);

    // Fit content to view all data
    if (this.chart) {
      this.chart.timeScale().fitContent();
    }
  }

  private handleResize = (): void => {
    if (this.chart && this.chartContainer) {
      this.chart.applyOptions({
        width: this.chartContainer.nativeElement.clientWidth,
      });
    }
  };

  ngOnChanges(): void {
    if (this.series) {
      this.updateChartData();
    }
  }
}
