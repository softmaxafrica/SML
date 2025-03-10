import { Component, OnInit } from '@angular/core';
import { PRIME_COMPONENTS } from '../../shared-modules';
 import { PrimeNG } from 'primeng/config';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-dashboard',
  imports: [PRIME_COMPONENTS],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  constructor(private config: PrimeNG, private translateService: TranslateService) {}


  ngOnInit(): void {

    //this.reloadPage();
  }


  reloadPage() {
    window.location.reload();
  }

}
