import { DrawerModule } from 'primeng/drawer';
import { ButtonModule } from 'primeng/button';
import { Ripple } from 'primeng/ripple';
import { AvatarModule } from 'primeng/avatar';
import { StyleClass } from 'primeng/styleclass';
import { RouterOutlet, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ToolbarModule } from 'primeng/toolbar';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { TranslateModule } from '@ngx-translate/core'; // Import translation module
import { TableModule } from 'primeng/table';
import { TableColumn } from './models/components/table-columns';  
import { PickListModule } from 'primeng/picklist';
import { FieldsetModule } from 'primeng/fieldset';
import { DialogModule } from 'primeng/dialog';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { TagModule } from 'primeng/tag';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { DividerModule } from 'primeng/divider';
import { SplitButtonModule } from 'primeng/splitbutton';
import { LoadingComponent } from './loading/loading.component';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';
import { TabsModule } from 'primeng/tabs';
import { TabMenuModule } from 'primeng/tabmenu';
import { TabViewModule } from 'primeng/tabview';
import { CarouselModule } from 'primeng/carousel';
import { DockModule } from 'primeng/dock';
import { ReactiveFormsModule } from '@angular/forms'; // Import this
import { MultiSelectModule } from 'primeng/multiselect';
import { PasswordModule } from 'primeng/password'; // Import PasswordModule

import { ChipModule } from 'primeng/chip';
import { FileUploadModule } from 'primeng/fileupload';

import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { CheckboxModule } from 'primeng/checkbox';
import { DatePickerModule } from 'primeng/datepicker';

export const PRIME_COMPONENTS = [
  DockModule,
  PasswordModule,
 MultiSelectModule ,
 FileUploadModule,
  ReactiveFormsModule,
  DatePickerModule,
    CheckboxModule,
  DrawerModule,
  CarouselModule,
  ChipModule,
  ToggleSwitchModule,
  TabViewModule,
  TabMenuModule,
  ButtonModule,
  Ripple,
  AvatarModule,
  StyleClass,
  RouterModule,
  RouterOutlet,
  CommonModule,
  CardModule,
  ToolbarModule,
  BreadcrumbModule,
  TranslateModule,
  TableModule,
  PickListModule,
  FieldsetModule,
  DialogModule,
  SelectModule,
  InputTextModule,
  FormsModule,
  TagModule,
  ProgressSpinnerModule,
  DividerModule,
  SplitButtonModule,
  LoadingComponent,
  MessageModule,
  MessagesModule,
  ToastModule,
  TabsModule

 ];
