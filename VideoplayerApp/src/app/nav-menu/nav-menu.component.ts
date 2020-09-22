import { Component, OnInit } from '@angular/core';
import { SignalrService } from '../signalr.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;

  constructor(public signalRService: SignalrService, private http: HttpClient) { }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addNumberOfRowsToHeader();
    //this.startHttp();
  }

  //private startHttp = () => {
  //  this.http.get('http://localhost:5000/api/signalr')
  //    .subscribe(res => { console.log(res) })
  //}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
