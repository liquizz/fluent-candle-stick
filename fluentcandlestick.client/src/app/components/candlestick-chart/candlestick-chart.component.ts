import { Component, Input, OnInit, ElementRef, ViewChild, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { createChart, IChartApi, CandlestickSeries, HistogramSeries, ColorType, DeepPartial, TimeChartOptions } from 'lightweight-charts';
import { CandleStick } from '../../models/candlestick.model';

@Component({
  selector: 'app-candlestick-chart',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="chart-wrapper">
      <div class="chart-toolbar">
        <div class="chart-title">Stock Price Movement</div>
        <div class="chart-controls">
          <button class="chart-btn" (click)="zoomIn()">Zoom In</button>
          <button class="chart-btn" (click)="zoomOut()">Zoom Out</button>
          <button class="chart-btn" (click)="resetZoom()">Reset</button>
        </div>
      </div>
      <div class="chart-container" #chartContainer></div>
    </div>
  `,
  styles: [`
    .chart-wrapper {
      width: 100%;
      background-color: white;
      border-radius: 6px;
      overflow: hidden;
    }

    .chart-toolbar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 12px 16px;
      background-color: #f8f9fa;
      border-bottom: 1px solid #eaeaea;
    }

    .chart-title {
      font-weight: 600;
      color: #323130;
    }

    .chart-controls {
      display: flex;
      gap: 8px;
    }

    .chart-btn {
      padding: 6px 12px;
      background-color: white;
      border: 1px solid #d2d2d2;
      border-radius: 4px;
      font-size: 12px;
      cursor: pointer;
      transition: all 0.2s;
    }

    .chart-btn:hover {
      background-color: #f0f0f0;
      border-color: #c8c8c8;
    }

    .chart-container {
      width: 100%;
      height: 450px;
    }

    @media (max-width: 600px) {
      .chart-toolbar {
        flex-direction: column;
        align-items: flex-start;
        gap: 10px;
      }

      .chart-container {
        height: 350px;
      }
    }
  `]
})
export class CandlestickChartComponent implements OnDestroy {
  @ViewChild('chartContainer') chartContainer!: ElementRef;
  @Input() data: CandleStick[] = [];

  private chart?: IChartApi;
  private candlestickSeries?: any;
  private volumeSeries?: any;
  private zoomLevel: number = 1;

  ngAfterViewInit(): void {
    this.initChart();
  }

  ngOnDestroy(): void {
    if (this.chart) {
      this.chart.remove();
    }
    window.removeEventListener('resize', this.handleResize);
  }

  private initChart(): void {
    const chartOptions: DeepPartial<TimeChartOptions> = {
      layout: {
        textColor: '#333333',
        background: {
          type: ColorType.Solid,
          color: '#ffffff'
        },
        fontFamily: 'Segoe UI, sans-serif',
      },
      width: this.chartContainer.nativeElement.clientWidth,
      height: 450,
      grid: {
        vertLines: { color: '#f5f5f5' },
        horzLines: { color: '#f5f5f5' },
      },
      crosshair: {
        mode: 1,
        vertLine: {
          color: 'rgba(0, 120, 212, 0.5)',
          width: 1,
          labelBackgroundColor: '#0078d4',
        },
        horzLine: {
          color: 'rgba(0, 120, 212, 0.5)',
          width: 1,
          labelBackgroundColor: '#0078d4',
        },
      },
      rightPriceScale: {
        borderColor: '#e1e5eb',
        scaleMargins: {
          top: 0.1,
          bottom: 0.2,
        },
      },
      timeScale: {
        borderColor: '#e1e5eb',
        timeVisible: true,
        secondsVisible: false,
        rightOffset: 5,
        barSpacing: 12,
        fixLeftEdge: true,
        lockVisibleTimeRangeOnResize: true,
      }
    };

    this.chart = createChart(this.chartContainer.nativeElement, chartOptions);

    this.candlestickSeries = this.chart.addSeries(CandlestickSeries, {
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
    if (!this.candlestickSeries || !this.data || this.data.length === 0) return;

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

    this.candlestickSeries.setData(chartData);

    // Add volume series below the price chart
    if (this.chart && this.data[0]?.volume !== undefined) {
      // Remove previous volume series if it exists
      if (this.volumeSeries) {
        this.chart.removeSeries(this.volumeSeries);
      }

      this.volumeSeries = this.chart.addSeries(HistogramSeries, {
        color: '#26a69a',
        priceScaleId: 'volume',
      });

      const volumeData = this.data.map(candle => {
        const timestamp = Math.floor(new Date(candle.time).getTime() / 1000);
        return {
          time: timestamp,
          value: candle.volume,
          color: candle.close >= candle.open ? '#26a69a80' : '#ef535080'
        };
      });

      this.volumeSeries.setData(volumeData);

      // Adjust price scale for volume
      this.chart.priceScale('volume').applyOptions({
        scaleMargins: {
          top: 0.8,
          bottom: 0,
        },
      });
    }

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

  // Chart control methods
  public zoomIn(): void {
    if (this.chart && this.zoomLevel < 3) {
      this.zoomLevel += 0.25;
      this.chart.timeScale().applyOptions({
        barSpacing: 12 * this.zoomLevel,
      });
    }
  }

  public zoomOut(): void {
    if (this.chart && this.zoomLevel > 0.5) {
      this.zoomLevel -= 0.25;
      this.chart.timeScale().applyOptions({
        barSpacing: 12 * this.zoomLevel,
      });
    }
  }

  public resetZoom(): void {
    if (this.chart) {
      this.zoomLevel = 1;
      this.chart.timeScale().applyOptions({
        barSpacing: 12,
      });
      this.chart.timeScale().fitContent();
    }
  }

  ngOnChanges(): void {
    if (this.candlestickSeries) {
      this.updateChartData();
    }
  }
}
