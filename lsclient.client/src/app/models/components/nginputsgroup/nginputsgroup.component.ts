import { Component, Input } from '@angular/core';
import { PRIME_COMPONENTS } from '../../../shared-modules';

@Component({
  selector: 'app-nginputsgroup',
  imports: [PRIME_COMPONENTS],
  templateUrl: './nginputsgroup.component.html',
  styleUrl: './nginputsgroup.component.css'
})
export class NginputsgroupComponent {
  @Input() fields: any[] = [];   

}
