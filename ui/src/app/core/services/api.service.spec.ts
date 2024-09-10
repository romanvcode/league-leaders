import { HttpClient } from '@angular/common/http';
import { Match } from '@core/models/match.model';
import { Standing } from '@core/models/standing.model';
import { of } from 'rxjs';
import { ApiService } from './api.service';

describe('ApiService', () => {
  let httpClientSpy: jasmine.SpyObj<HttpClient>;
  let service: ApiService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    service = new ApiService(httpClientSpy);
  });

  it('should return expected standings (HttpClient called once)', (done: DoneFn) => {
    const expectedStandings: Standing[] = [
      {
        id: 1,
        name: 'A',
        place: 1,
        matchesPlayed: 1,
        wins: 1,
        draws: 0,
        losses: 0,
        goalsFor: 1,
        goalsAgainst: 0,
        points: 3,
      },
      {
        id: 2,
        name: 'B',
        place: 2,
        matchesPlayed: 1,
        wins: 0,
        draws: 1,
        losses: 0,
        goalsFor: 1,
        goalsAgainst: 1,
        points: 1,
      },
    ];
    httpClientSpy.get.and.returnValue(of(expectedStandings));
    service.getStandings().subscribe({
      next: (standings) => {
        expect(standings).toEqual(expectedStandings);
        done();
      },
      error: done.fail,
    });
    expect(httpClientSpy.get.calls.count()).toBe(1);
  });

  it('should return an error when the server returns a 404', () => {
    httpClientSpy.get.and.returnValue(of({}));
    service.getStandings().subscribe({
      next: () => {},
      error: (error: any) => {
        expect(error).toEqual(error);
      },
    });
  });

  it('should return expected mathches (HttpClient called once)', (done: DoneFn) => {
    const expectedMatches: Match[] = [
      {
        id: 1,
        homeTeam: { name: 'A' },
        awayTeam: { name: 'B' },
        date: new Date().getDate().toString(),
        homeTeamScore: 1,
        awayTeamScore: 0,
      },
    ];
    httpClientSpy.get.and.returnValue(of(expectedMatches));
    service.getMathces().subscribe({
      next: (matches) => {
        expect(matches).toEqual(expectedMatches);
        done();
      },
      error: done.fail,
    });
    expect(httpClientSpy.get.calls.count()).toBe(1);
  });

  it('should return an error when the server returns a 404', () => {
    httpClientSpy.get.and.returnValue(of({}));
    service.getMathces().subscribe({
      next: () => {},
      error: (error: any) => {
        expect(error).toEqual(error);
      },
    });
  });
});
