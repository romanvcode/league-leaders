import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpClient, HttpHandler } from '@angular/common/http';
import { ApiService } from '@core/services/api.service';
import { NextMatchComponent } from './next-match.component';

describe('NextMatchComponent', () => {
  let component: NextMatchComponent;
  let fixture: ComponentFixture<NextMatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NextMatchComponent],
      providers: [ApiService, HttpClient, HttpHandler],
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
    expect(component.countdown).toBeDefined();
  });

  it('should not call the countdown method if no matches', () => {
    spyOn(component, 'countdown');
    expect(component.countdown).not.toHaveBeenCalled();
  });
});
