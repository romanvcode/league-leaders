import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { provideAnimations } from '@angular/platform-browser/animations';
import { Prediction } from '@core/models/prediction.model';
import { ApiService } from '@core/services/api.service';
import { of } from 'rxjs';
import { PredictionFormComponent } from './prediction-form.component';

describe('PredictionFormComponent', () => {
  let component: PredictionFormComponent;
  let fixture: ComponentFixture<PredictionFormComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<PredictionFormComponent>>;

  beforeEach(async () => {
    const dialogRefMock = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [PredictionFormComponent],
      providers: [
        ApiService,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideAnimations(),
        { provide: MatDialogRef, useValue: dialogRefMock },
        {
          provide: MAT_DIALOG_DATA,
          useValue: {
            id: 1,
            homeTeam: { name: 'Team A' },
            awayTeam: { name: 'Team B' },
            homeTeamScore: 0,
            awayTeamScore: 0,
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PredictionFormComponent);
    component = fixture.componentInstance;
    dialogRefSpy = TestBed.inject(MatDialogRef) as jasmine.SpyObj<
      MatDialogRef<PredictionFormComponent>
    >;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set matchId, homeTeamScore, and awayTeamScore', () => {
    expect(component.matchId).toBe(1);
    expect(component.homeTeamScore).toBe(0);
    expect(component.awayTeamScore).toBe(0);
  });

  it('should close the dialog on successful submit', () => {
    const apiService = TestBed.inject(ApiService);

    const prediction: Prediction = {
      id: 1,
      matchId: 1,
      homeTeamScore: 1,
      awayTeamScore: 2,
    };

    spyOn(apiService, 'createPrediction').and.returnValue(of(prediction));

    component.onSubmit();

    expect(apiService.createPrediction).toHaveBeenCalled();

    expect(dialogRefSpy.close).toHaveBeenCalled();
  });

  it('should not close the dialog on error', () => {
    component.isError = true;

    component.onSubmit();

    expect(dialogRefSpy.close).not.toHaveBeenCalled();
  });
});
