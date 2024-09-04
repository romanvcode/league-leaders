import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StandingTableComponent } from './standing-table.component';

describe('StandingTableComponent', () => {
  let component: StandingTableComponent;
  let fixture: ComponentFixture<StandingTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StandingTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StandingTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
