import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideAnimations } from '@angular/platform-browser/animations';
import { Router } from '@angular/router';
import { ApiService } from '@core/services/api.service';
import { SearchInputComponent } from './search-input.component';

describe('SearchInputComponent', () => {
  let component: SearchInputComponent;
  let fixture: ComponentFixture<SearchInputComponent>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const routerSpyObj = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [SearchInputComponent],
      providers: [
        ApiService,
        provideAnimations(),
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: Router, useValue: routerSpyObj },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(SearchInputComponent);
    component = fixture.componentInstance;
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should clear the #search and #searchResults', () => {
    component.value = 'Team A';
    component.searchResults = [
      {
        id: 1,
        name: 'Team A',
        abbreviation: 'A',
        country: 'Country A',
        stadium: 'Stadium A',
        manager: 'Manager A',
        players: [],
      },
    ];

    component.clearSearch();
    fixture.detectChanges();

    expect(component.value).toBe('');
    expect(component.searchResults.length).toBe(0);
  });

  it('should navigate to the #selectedTeam', () => {
    const selectedTeam = {
      id: 1,
      name: 'Team A',
      abbreviation: 'A',
      country: 'Country A',
      stadium: 'Stadium A',
      manager: 'Manager A',
      players: [],
    };

    component.onSelect(selectedTeam);
    fixture.detectChanges();

    expect(routerSpy.navigate).toHaveBeenCalledWith(['/team'], {
      queryParams: { id: selectedTeam.id },
    });
  });
});
