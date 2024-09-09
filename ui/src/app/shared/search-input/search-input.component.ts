import { Component, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-search-input',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './search-input.component.html',
  styleUrl: './search-input.component.css',
})
export class SearchInputComponent {
  search = output<string>();

  value: string = '';

  onSearch() {
    this.search.emit(this.value);
  }
}
