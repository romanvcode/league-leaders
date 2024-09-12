import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ApiService } from '@core/services/api.service';
import { TeamInfoComponent } from './team-info.component';

describe('TeamInfoComponent', () => {
  let component: TeamInfoComponent;
  let fixture: ComponentFixture<TeamInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamInfoComponent],
      providers: [ApiService, provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(TeamInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a teamId property', () => {
    expect(component.teamId).toBeDefined();
  });

  it('should have a team property', () => {
    expect(component.team).toBeDefined();
  });

  it('should have a matches property', () => {
    expect(component.matches).toBeDefined();
  });
});
