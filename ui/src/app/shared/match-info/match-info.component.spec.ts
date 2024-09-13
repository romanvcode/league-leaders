import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { Match } from '@core/models/match.model';
import { ApiService } from '@core/services/api.service';
import { MatchInfoComponent } from './match-info.component';

describe('MatchInfoComponent', () => {
  let component: MatchInfoComponent;
  let fixture: ComponentFixture<MatchInfoComponent>;

  const mockMatch: Match = {
    id: 1,
    date: '2021-06-01T00:00:00Z',
    homeTeam: {
      id: 1,
      name: 'Home Team',
      abbreviation: 'HT',
      country: 'HTC',
      manager: 'HTM',
      stadium: 'HTS',
      players: [],
    },
    awayTeam: {
      id: 2,
      name: 'Away Team',
      abbreviation: 'AT',
      country: 'ATC',
      manager: 'ATM',
      stadium: 'ATS',
      players: [],
    },
    homeTeamScore: 1,
    awayTeamScore: 2,
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatchInfoComponent],
      providers: [ApiService, provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(MatchInfoComponent);
    component = fixture.componentInstance;
    component.match = mockMatch;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render match info title', () => {
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('#match-info-title').textContent).toContain(
      'Home Team 1 - 2 Away Team'
    );
  });
});
