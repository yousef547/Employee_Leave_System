import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <nav class="navbar navbar-dark bg-primary px-3">
      <span class="navbar-brand">🏢 نظام إجازات الموظفين</span>
    </nav>
    <router-outlet></router-outlet>
  `
})
export class AppComponent {}
