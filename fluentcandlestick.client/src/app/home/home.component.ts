import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandleStickService } from '../services/candlestick.service';
import { CandleStick } from '../models/candlestick.model';
import { CandlestickChartComponent } from '../components/candlestick-chart/candlestick-chart.component';
import { FileImportComponent } from '../components/file-import/file-import.component';
import { CandlestickTableComponent } from '../components/candlestick-table/candlestick-table.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    CandlestickChartComponent,
    FileImportComponent,
    CandlestickTableComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  candleStickData: CandleStick[] = [];
  isLoading = false;
  hasData = false;

  constructor(private candleStickService: CandleStickService) {}

  ngOnInit(): void {
    this.loadData();
  }

  onDataImported(): void {
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.candleStickService.getCandleStickData().subscribe({
      next: (data) => {
        this.candleStickData = data;
        this.hasData = data.length > 0;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  getRowClass(candleStick: CandleStick): string {
    return candleStick.isUp ? 'price-up' : 'price-down';
  }
}
