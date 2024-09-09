import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Match } from '@core/models/match.model';
import { ApiService } from '@core/services/api.service';

@Component({
  selector: 'app-next-match',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './next-match.component.html',
  styleUrl: './next-match.component.css',
})
export class NextMatchComponent implements OnInit {
  match = signal<Match | undefined>(undefined);
  error = signal('');
  countdown = signal('');

  private apiService = inject(ApiService);
  private destroyRef = inject(DestroyRef);

  ngOnInit(): void {
    const subscription = this.apiService.getMathces().subscribe({
      next: (matches) => {
        this.match.set(matches.shift());
        this.startCountdown(new Date(this.match()!.date));
      },
      error: (error) => {
        console.error(error);
        this.error.set('An error occurred while fetching matches');
      },
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  startCountdown(startTime: Date): void {
    const interval = setInterval(() => {
      const now = new Date().getTime();
      const distance = startTime.getTime() - now;

      const hours = Math.floor(
        (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
      );
      const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((distance % (1000 * 60)) / 1000);
      this.countdown.set(`${hours}h ${minutes}m ${seconds}s`);
    }, 1000);
  }
}
