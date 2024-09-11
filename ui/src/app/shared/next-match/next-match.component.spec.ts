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

  it('should have a countdown property', () => {
    expect(component.timeLeft$).toBeDefined();
  });

  it('should have a nextMatch property', () => {
    expect(component.nextMatch$).toBeDefined();
  });
});
