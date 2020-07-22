import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Event, NavigationEnd } from '@angular/router';
import { SessiondataService } from './sessiondata.service';
import { HttpClient } from '@angular/common/http';
import { v4 as uuid } from 'uuid';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})


export class VideoComponent implements OnInit
{
  player: YT.Player;
  private id: string;
  private origin: "https://localhost:44322";
  ipAdress: any;
  idSession: string;
  state: string;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private sessiondata: SessiondataService,
    private http: HttpClient){
    this.http.get('https://jsonip.com').subscribe((ipOfNetwork) => this.ipAdress = ipOfNetwork['ip']);
  }
  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.idSession = uuid();
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationEnd) {
        console.log("Status:", "FINISHED");
        this.sessiondata.update({
          Id: this.idSession,
          Status: 'FINISHED',
          UserAdress: this.ipAdress,
          IdVideo: this.id
        }, this.idSession);
      }   
    });
  }

  savePlayer(player){
    this.player = player;
    console.log("player instance", this.player);
  }

  onStateChange(event){
    if (event.data == -1) {
      console.log("Status:", "UNSTARTED");
      this.sessiondata.create({
        Id: this.idSession,
        Status: 'UNSTARTED',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
    }
    else if (event.data == 0) {
      console.log("Status:", "ENDED");
      this.sessiondata.update({
        Id: this.idSession,
        Status: 'ENDED',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      }, this.idSession);
    }
    else if (event.data == 1) {
      console.log("Status:", "PLAY");
      this.sessiondata.update({
        Id: this.idSession,
        Status: 'PLAY',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      }, this.idSession);
    }
    else if (event.data == 2) {
      console.log("Status:", "PAUSED");
      this.sessiondata.update({
        Id: this.idSession,
        Status: 'PAUSE',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      }, this.idSession);
    }
    else if (event.data == 3) {
      console.log("Status:", "BUFFERING");
      this.sessiondata.update({
        Id: this.idSession,
        Status: 'BUFFERING',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      }, this.idSession);
    }
    else if (event.data == 5) {
      console.log("Status:", "VIDEO_CUED");
      this.sessiondata.update({
        Id: this.idSession,
        Status: 'VIDEO_CUED',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      }, this.idSession);
    }
  }
}


