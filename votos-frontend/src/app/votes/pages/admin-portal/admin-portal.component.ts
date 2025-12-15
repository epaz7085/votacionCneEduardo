import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { NotificationService, Notification } from '../../services/notification.service';

@Component({
  selector: 'app-admin-portal',
  standalone: true,
  imports: [CommonModule, RouterModule], 
  templateUrl: './admin-portal.component.html',
  styleUrls: ['./admin-portal.component.css']
})
export class AdminPortalComponent implements OnInit {

  notifications: Notification[] = [];

  constructor(
    private notificationService: NotificationService,
    private router: Router
  ) {}

  darkMode = false;

  ngOnInit(): void {
  const savedMode = localStorage.getItem('darkMode');
  this.darkMode = savedMode === 'true';

  this.notificationService.getAdminNotifications()
    .subscribe(data => {
      this.notifications = data;
    });
  }

  toggleDarkMode() {
  this.darkMode = !this.darkMode;
  localStorage.setItem('darkMode', this.darkMode.toString());
  }


  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
  
  
}
