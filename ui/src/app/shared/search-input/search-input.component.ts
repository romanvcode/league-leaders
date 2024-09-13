import { NgForOf, NgIf } from '@angular/common';
import { Component, EventEmitter, OnDestroy, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ApiService } from '@core/services/api.service';
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-search-input',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatAutocompleteModule,
    NgIf,
    NgForOf,
  ],
  templateUrl: './search-input.component.html',
  styleUrl: './search-input.component.css',
})
export class SearchInputComponent implements OnDestroy {
  private destroy$ = new Subject<void>();

  @Output() search = new EventEmitter<string>();

  value: string = '';
  searchResults: string[] = [];

  constructor(private apiService: ApiService) {}

  onSearch() {
    if (this.value) {
      this.apiService
        .getTeamsBySearchTerm(this.value)
        .pipe(takeUntil(this.destroy$))
        .pipe(debounceTime(300), distinctUntilChanged())
        .subscribe((teams) => {
          this.searchResults = teams.map((team) => team.name);
        });
    } else {
      this.clearSearch();
    }
  }

  onSelect(result: string) {
    this.search.emit(result);
    this.value = result;
  }

  clearSearch() {
    this.value = '';
    this.searchResults = [];
    this.search.emit('');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
