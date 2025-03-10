import { Component, OnInit } from '@angular/core';
import { PRIME_COMPONENTS } from '../../shared-modules';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrl:   '../../app.component.css', 
imports: [PRIME_COMPONENTS],
 })
export class LandingComponent implements OnInit {
  showWelcomeContent = true;

  constructor() {}

  ngOnInit() {
 
  }

  hideWelcomeContent() {
    this.showWelcomeContent = false;
  }
}
