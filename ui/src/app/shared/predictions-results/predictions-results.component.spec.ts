import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PredictionsResultsComponent } from './predictions-results.component';

describe('PredictionsResultsComponent', () => {
  let component: PredictionsResultsComponent;
  let fixture: ComponentFixture<PredictionsResultsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PredictionsResultsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PredictionsResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
