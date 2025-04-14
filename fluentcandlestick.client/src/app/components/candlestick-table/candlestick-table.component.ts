import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandleStick } from '../../models/candlestick.model';

@Component({
  selector: 'app-candlestick-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './candlestick-table.component.html',
  styleUrl: './candlestick-table.component.css'
})
export class CandlestickTableComponent {
  @Input() candleStickData: CandleStick[] = [];

  getRowClass(candleStick: CandleStick): string {
    return candleStick.isUp ? 'price-up' : 'price-down';
  }
}
