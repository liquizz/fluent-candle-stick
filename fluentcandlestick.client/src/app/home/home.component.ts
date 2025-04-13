import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CandleStickService } from '../services/candlestick.service';
import { CandleStick } from '../models/candlestick.model';
import { CandlestickChartComponent } from '../components/candlestick-chart/candlestick-chart.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule, CandlestickChartComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.less'
})
export class HomeComponent implements OnInit {
  candleStickData: CandleStick[] = [];
  file: File | null = null;
  isLoading = false;
  hasData = false;
  error: string | null = null;

  constructor(private candleStickService: CandleStickService) {}

  ngOnInit(): void {
    this.loadData();
  }

  onFileSelected(event: Event): void {
    const element = event.target as HTMLInputElement;
    if (element.files && element.files.length > 0) {
      this.file = element.files[0];
    }
  }

  uploadFile(): void {
    if (!this.file) {
      this.error = 'Please select a file';
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.candleStickService.importCsvData(this.file).subscribe({
      next: () => {
        this.isLoading = false;
        this.file = null;
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.error = 'Failed to import file. Please check the file format.';
        console.error(err);
      }
    });
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
