import { Component, ViewChild, ChangeDetectorRef, OnInit } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { MenuItem } from 'primeng/api';
import { PrimeNG } from 'primeng/config';
import { PRIME_COMPONENTS } from './shared-modules';
import { AppRoutingModule } from './app.routes';
import { BreadcrumbService } from './services/breadcrumb.service';
import { MenuModule, Menu } from 'primeng/menu';
import { AuthService } from './auth/auth.service';
import { ToolbarModule } from 'primeng/toolbar';
import { SecUser } from './models/SecUser';

interface CustomMenuItem {
  label: string;
  routerLink?: string;
  icon?: string;
  badge?: number;
}

interface BreadcrumbItem {
  label: string;
  routerLink?: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, PRIME_COMPONENTS, MenuModule, ToolbarModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'lg';
  isCollapsed: boolean = true;
  isLoggedIn: boolean = false;

  breadcrumbItems: BreadcrumbItem[] = [];
  profileItems: MenuItem[] = [];
  menuItems: CustomMenuItem[] = [];
  loggedInUser: SecUser | null = null; 
  @ViewChild('profileMenu') profileMenu!: Menu;

  constructor(
    public authService: AuthService,
    private primeng: PrimeNG,
    private cdr: ChangeDetectorRef,
    public breadcrumbService: BreadcrumbService,
    private router: Router,
   ) {
    this.profileItems = [
      { label: 'Profile', icon: 'pi pi-user', command: () => this.viewProfile() },
      { label: 'Settings', icon: 'pi pi-cog', command: () => this.openSettings() },
      { label: 'Logout', icon: 'pi pi-sign-out', command: () => this.logout() }
    ];
  }

  updateMenuItems() {
    if (this.isLoggedIn) {
      const userRole = this.authService.getUserRole(); // Get the user's role
  
      if (userRole === 'CUSTOMER') {
        // Menu for CUSTOMER role
        this.menuItems = [
          { label: 'Dashboard', routerLink: '/home/dashboard', icon: 'pi pi-home' },
          { label: 'Requests', routerLink: '/home/customer/requests', icon: 'pi pi-briefcase' },
          { label: 'Invoices', routerLink: '/home/customer/billing/invoices', icon: 'pi pi-file' }
        ];
      } else if (userRole === 'COMPANY') {
        // Menu for COMPANY role
        this.menuItems = [
          { label: 'Dashboard', routerLink: '/home/dashboard', icon: 'pi pi-home' },
          { label: 'Requests', routerLink: '/home/request', icon: 'pi pi-briefcase' },
          { label: 'Invoices', routerLink: '/home/billing/invoices', icon: 'pi pi-file' },
          { label: 'Payments', routerLink: '/home/billing/payments', icon: 'pi pi-credit-card' },
          { label: 'Trucks', routerLink: '/home/trucks', icon: 'pi pi-truck' },
          { label: 'Drivers', routerLink: '/home/drivers', icon: 'pi pi-user-plus' }
        ];
      } else {
        // Default menu for other roles (if any)
        this.menuItems = [
          { label: 'Dashboard', routerLink: '/home/dashboard', icon: 'pi pi-home' }
        ];
      }
    } else {
      // If not logged in, show login and registration only
      this.menuItems = [
        { label: 'Login', routerLink: '/account/login', icon: 'pi pi-sign-in' }
      ];
    }
  }

  viewProfile() {
    console.log('View Profile');
  }

  openSettings() {
    console.log('Open Settings');
  }

  logout() {
    this.authService.logout();
  }

  ngOnInit() {
    this.primeng.ripple.set(true);

    this.authService.loggedInUser$.subscribe(user => {
      this.loggedInUser = user; // Update the logged-in user details
      this.isLoggedIn = this.authService.isLoggedIn();
      this.updateMenuItems();

    });

 

    // Update breadcrumbs whenever the route changes
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.breadcrumbItems = this.getBreadcrumbs(event.urlAfterRedirects);
      });
  }

  // Toggle the Drawer (sidebar)
  toggleDrawer() {
    this.isCollapsed = !this.isCollapsed;
  }

  // Toggle profile menu
  toggleProfileMenu(event: Event) {
    this.profileMenu.toggle(event);
  }

  // Get breadcrumbs based on the current URL
  getBreadcrumbs(currentUrl: string): BreadcrumbItem[] {
    const homeItem: BreadcrumbItem = { label: '', routerLink: '/' };

    for (const item of this.menuItems) {
      if (item.routerLink === currentUrl) {
        return [
          homeItem,
          { label: item.label, routerLink: item.routerLink }
        ];
      }
    }
    return [homeItem, { label: 'Home', routerLink: '/' }];
  }
}