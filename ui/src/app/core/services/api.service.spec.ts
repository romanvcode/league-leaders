import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Match } from '@core/models/match.model';
import { Standing } from '@core/models/standing.model';
import { of, throwError } from 'rxjs';
import { ApiService } from './api.service';

describe('ApiService', () => {
  let httpClientSpy: jasmine.SpyObj<HttpClient>;
  let service: ApiService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    service = new ApiService(httpClientSpy);
  });

  it('#getStandings should return expected standings (HttpClient called once)', (done: DoneFn) => {
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

  it('#getStandings should return an error when the server returns an error', (done: DoneFn) => {
    const expectedError = new HttpErrorResponse({});

    httpClientSpy.get.and.returnValue(throwError(() => expectedError));

    service.getStandings().subscribe({
      next: () => done.fail,
      error: (error) => {
        expect(error).toEqual(expectedError);
        done();
      },
    });
  });

  it('#getMatches should return expected mathches (HttpClient called once)', (done: DoneFn) => {
    const expectedMatches: Match[] = [
      {
        id: 1,
        homeTeam: {
          id: 1,
          name: 'Team A',
          abbreviation: 'A',
          country: 'Country A',
          stadium: 'Stadium A',
          manager: 'Manager A',
          players: [],
        },
        awayTeam: {
          id: 2,
          name: 'Team B',
          abbreviation: 'B',
          country: 'Country B',
          stadium: 'Stadium B',
          manager: 'Manager B',
          players: [],
        },
        date: new Date().getDate().toString(),
        homeTeamScore: 1,
        awayTeamScore: 0,
      },
    ];
    httpClientSpy.get.and.returnValue(of(expectedMatches));
    service.getMatches().subscribe({
      next: (matches) => {
        expect(matches).toEqual(expectedMatches);
        done();
      },
      error: done.fail,
    });
    expect(httpClientSpy.get.calls.count()).toBe(1);
  });

  it('#getMatches should return an error when the server returns an error', (done: DoneFn) => {
    const expectedError = new HttpErrorResponse({});

    httpClientSpy.get.and.returnValue(throwError(() => expectedError));

    service.getMatches().subscribe({
      next: () => done.fail,
      error: (error) => {
        expect(error).toEqual(expectedError);
        done();
      },
    });
  });

  it('#getTeamMatches should return expected mathches (HttpClient called once)', (done: DoneFn) => {
    const expectedMatches: Match[] = [
      {
        id: 1,
        homeTeam: {
          id: 1,
          name: 'Team A',
          abbreviation: 'A',
          country: 'Country A',
          stadium: 'Stadium A',
          manager: 'Manager A',
          players: [],
        },
        awayTeam: {
          id: 2,
          name: 'Team B',
          abbreviation: 'B',
          country: 'Country B',
          stadium: 'Stadium B',
          manager: 'Manager B',
          players: [],
        },
        date: new Date().getDate().toString(),
        homeTeamScore: 1,
        awayTeamScore: 0,
      },
    ];
    httpClientSpy.get.and.returnValue(of(expectedMatches));
    service.getTeamMatches(1).subscribe({
      next: (matches) => {
        expect(matches).toEqual(expectedMatches);
        done();
      },
      error: done.fail,
    });
    expect(httpClientSpy.get.calls.count()).toBe(1);
  });

  it('#getTeamMatches should return an error when the server returns an error', (done: DoneFn) => {
    const expectedError = new HttpErrorResponse({});

    httpClientSpy.get.and.returnValue(throwError(() => expectedError));

    service.getTeamMatches(1).subscribe({
      next: () => done.fail,
      error: (error) => {
        expect(error).toEqual(expectedError);
        done();
      },
    });
  });

  it('#getTeamsBySearchTerm should return expected teams (HttpClient called once)', (done: DoneFn) => {
    const expectedTeams = [
      {
        id: 1,
        name: 'Team A',
        abbreviation: 'A',
        country: 'Country A',
        stadium: 'Stadium A',
        manager: 'Manager A',
        players: [],
      },
      {
        id: 2,
        name: 'Team B',
        abbreviation: 'B',
        country: 'Country B',
        stadium: 'Stadium B',
        manager: 'Manager B',
        players: [],
      },
    ];
    httpClientSpy.get.and.returnValue(of(expectedTeams));
    service.getTeamsBySearchTerm('Team').subscribe({
      next: (teams) => {
        expect(teams).toEqual(expectedTeams);
        done();
      },
      error: done.fail,
    });
    expect(httpClientSpy.get.calls.count()).toBe(1);
  });

  it('#getTeamsBySearchTerm should return an error when the server returns an error', (done: DoneFn) => {
    const expectedError = new HttpErrorResponse({});

    httpClientSpy.get.and.returnValue(throwError(() => expectedError));

    service.getTeamsBySearchTerm('Team').subscribe({
      next: () => done.fail,
      error: (error) => {
        expect(error).toEqual(expectedError);
        done();
      },
    });
  });
});
