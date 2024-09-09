import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Match } from '@core/models/match.model';
import { ApiService } from '@core/services/api.service';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-next-match',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './next-match.component.html',
  styleUrl: './next-match.component.css',
})
export class NextMatchComponent implements OnInit, OnDestroy {
  match = signal<Match | undefined>(undefined);
  error = signal('');
  subscription = signal<Subscription | undefined>(undefined);
  interval = signal<Subscription | undefined>(undefined);
  countdown = signal('');

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.subscription.set(
      this.apiService.getMathces().subscribe({
        next: (matches) => {
          this.match.set(matches.shift());
          this.startCountdown(new Date(this.match()!.date));
        },
        error: (error) => {
          console.error(error);
          this.error.set('An error occurred while fetching matches');
        },
      })
    );
  }

  startCountdown(startTime: Date): void {
    this.interval.set(
      interval(1000).subscribe(() => {
        const now = new Date().getTime();
        const distance = startTime.getTime() - now;

        const days = Math.floor(distance / (1000 * 60 * 60 * 24));
        const hours = Math.floor(
          (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
        );
        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((distance % (1000 * 60)) / 1000);
        this.countdown.set(`${days}d ${hours}h ${minutes}m ${seconds}s`);
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription()?.unsubscribe();
    this.interval()?.unsubscribe();
  }
}
