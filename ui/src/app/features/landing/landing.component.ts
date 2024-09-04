import { Component } from '@angular/core';
import { StandingTableComponent } from "../../shared/standing-table/standing-table.component";

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [StandingTableComponent],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent {

}
