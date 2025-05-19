import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { Team } from '@core/models/team.model';
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
  ],
  templateUrl: './search-input.component.html',
  styleUrl: './search-input.component.css',
})
export class SearchInputComponent implements OnDestroy {
  private destroy$ = new Subject<void>();

  value: string = '';
  searchResults: Team[] = [];

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

  onSearch() {
    if (this.value) {
      this.apiService
        .getTeamsBySearchTerm(this.value)
        .pipe(takeUntil(this.destroy$))
        .pipe(debounceTime(300), distinctUntilChanged())
        .subscribe((teams) => {
          this.searchResults = teams;
        });
    } else {
      this.clearSearch();
    }
  }

  onSelect(result: Team) {
    this.value = result.name;
    this.router.navigate(['/team'], { queryParams: { id: result.id } });
  }

  clearSearch() {
    this.value = '';
    this.searchResults = [];
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
