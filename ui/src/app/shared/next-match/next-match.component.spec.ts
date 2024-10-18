import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ApiService } from '@core/services/api.service';
import { NextMatchComponent } from './next-match.component';

describe('NextMatchComponent', () => {
  let component: NextMatchComponent;
  let fixture: ComponentFixture<NextMatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NextMatchComponent],
      providers: [ApiService, provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NextMatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render #next-match after fetching data', () => {
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('#next-match')).not.toBeNull();
  });

  it('should not render #next-match on error', () => {
    component.isError = true;
    fixture.detectChanges();

    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('#next-match')).toBeNull();
  });
});
