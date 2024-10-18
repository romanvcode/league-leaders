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

  it('should render #team-info', () => {
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.team-overview')).not.toBeNull();
  });

  it('should not render #team-info on error', () => {
    component.isError = true;
    fixture.detectChanges();

    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.team-overview')).toBeNull();
  });
});
