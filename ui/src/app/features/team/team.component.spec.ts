import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ActivatedRoute, provideRouter, Router } from '@angular/router';
import { ApiService } from '@core/services/api.service';
import { of } from 'rxjs';
import { TeamComponent } from './team.component';

describe('TeamComponent', () => {
  let component: TeamComponent;
  let fixture: ComponentFixture<TeamComponent>;
  let router: Router;
  let expectedFeature: string;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamComponent],
      providers: [
        ApiService,
        provideAnimations(),
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([
          {
            path: 'team',
            component: TeamComponent,
          },
        ]),
        {
          provide: ActivatedRoute,
          useValue: { queryParams: of({ id: 1 }) },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(TeamComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    fixture.detectChanges();

    expectedFeature =
      fixture.nativeElement.querySelector('#current-feature').textContent;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a title', () => {
    expect(expectedFeature).toContain(component.currentFeature);
  });

  it('should set teamId from route parameters when navigating to /team/1', async () => {
    await router.navigate(['/team'], { queryParams: { id: 1 } });
    fixture.detectChanges();

    expect(component.teamId).toBe(1);
  });

  it('should render child components', () => {
    const compiled = fixture.nativeElement;

    expect(compiled.querySelector('app-next-match')).not.toBeNull();
    expect(compiled.querySelector('app-search-input')).not.toBeNull();
    expect(compiled.querySelector('app-team-info')).not.toBeNull();
  });
});
