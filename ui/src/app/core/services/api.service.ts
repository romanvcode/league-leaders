import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private http = inject(HttpClient);

  getStandings(): Observable<any> {
    return this.http.get(`https://localhost:7250/api/leaderboard/standings`);
  }

  // private httpClient = inject(HttpClient);
  // private destroyRef = inject(DestroyRef);

  // ngOnInit() {
  //   const subscription = this.httpClient
  //     .get('https://localhost:7250/api/leaderboard/standings')
  //     .subscribe({
  //       next: (resData) => {
  //         console.log(resData);
  //       },
  //       error: (error) => {
  //         console.error(error);
  //       },
  //     });

  //   this.destroyRef.onDestroy(() => {
  //     subscription.unsubscribe();
  //   });
  // }
}
