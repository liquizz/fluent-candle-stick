import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandleStickService } from '../../services/candlestick.service';

@Component({
  selector: 'app-file-import',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './file-import.component.html',
  styleUrl: './file-import.component.css'
})
export class FileImportComponent {
  @Output() dataImported = new EventEmitter<void>();

  file: File | null = null;
  isLoading = false;
  error: string | null = null;

  constructor(private candleStickService: CandleStickService) {}

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
        this.dataImported.emit();
      },
      error: (err) => {
        this.isLoading = false;
        this.error = 'Failed to import file. Please check the file format.';
        console.error(err);
      }
    });
  }
}
