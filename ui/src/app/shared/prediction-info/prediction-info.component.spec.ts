import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PredictionInfoComponent } from './prediction-info.component';

describe('PredictionInfoComponent', () => {
  let component: PredictionInfoComponent;
  let fixture: ComponentFixture<PredictionInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PredictionInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PredictionInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
