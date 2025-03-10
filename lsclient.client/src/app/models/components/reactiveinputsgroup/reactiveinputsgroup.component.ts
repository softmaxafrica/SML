import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { PRIME_COMPONENTS } from '../../../shared-modules';

@Component({
  selector: 'app-reactive-input-group',
  templateUrl: './reactiveinputsgroup.component.html',
  imports:[PRIME_COMPONENTS]
})
export class ReactiveInputGroupComponent {
  @Input() formGroup!: FormGroup; // Parent form group
  @Input() leftFields: any[] = []; // Fields for the left column
  @Input() rightFields: any[] = []; // Fields for the right column
  @Input() buttons: any[] = []; // Buttons for the footer
  @Output() buttonClick = new EventEmitter<string>(); // Emit button actions
  @Output() fileUpload = new EventEmitter<any>(); // Emit file upload events
  @Input() formObject: any; // The object to bind the form fields to (e.g., driverPayload)

 

  // Handle button clicks
  onButtonClick(action: string) {
    this.buttonClick.emit(action);
  }

  // Handle file upload
  onFileUpload(event: any, key: string) {
    const file = event.files[0];
    this.fileUpload.emit({ key, file });
  }
}