import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ApiService } from '@core/services/api.service';
import { LandingComponent } from './landing.component';

describe('LandingComponent', () => {
  let component: LandingComponent;
  let fixture: ComponentFixture<LandingComponent>;
  let expectedFeature: string;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LandingComponent],
      providers: [
        ApiService,
        provideAnimations(),
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(LandingComponent);
    component = fixture.componentInstance;
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

  it('should render child components', () => {
    const compiled = fixture.nativeElement;

    expect(compiled.querySelector('app-next-match')).not.toBeNull();
    expect(compiled.querySelector('app-search-input')).not.toBeNull();
    expect(compiled.querySelector('app-standing-table')).not.toBeNull();
  });
});
