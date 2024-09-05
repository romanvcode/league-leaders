import { Component } from '@angular/core';
import { StandingTableComponent } from '../../shared/standing-table/standing-table.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [StandingTableComponent, MatGridListModule, MatToolbarModule],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
})
export class LandingComponent {}
