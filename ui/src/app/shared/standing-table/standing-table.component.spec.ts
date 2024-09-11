import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ApiService } from '@core/services/api.service';
import { StandingTableComponent } from './standing-table.component';

describe('StandingTableComponent', () => {
  let component: StandingTableComponent;
  let fixture: ComponentFixture<StandingTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StandingTableComponent],
      providers: [ApiService, provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(StandingTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a standings property', () => {
    expect(component.dataSource).toBeDefined();
  });

  it('should set isFetching to true on ngOnInit', () => {
    component.ngOnInit();
    expect(component.isFetching).toBe(true);
  });
});
