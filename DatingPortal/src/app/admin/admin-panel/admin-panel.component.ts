import { Component, OnInit, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent implements OnInit {

  @ViewChild('adminTabs', { static: false }) adminTabs: TabsetComponent;
  selectTab(tabId: number) {
    this.adminTabs.tabs[tabId].active = true;
  }

  constructor() { }

  ngOnInit() {
  }

}
