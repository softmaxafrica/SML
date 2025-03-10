interface MenuItem {
    label: string;
    routerLink?: string;
    icon?: string;
    badge?: number;
    open?: boolean;
    children?: MenuItem[];
  }
  
  interface BreadcrumbItem {
    label: string;
    routerLink?: string;
  }