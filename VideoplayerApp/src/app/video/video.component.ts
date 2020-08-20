import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, Event, NavigationEnd } from '@angular/router';
import { SessiondataService } from './sessiondata.service';
import { HttpClient } from '@angular/common/http';
import { v4 as uuid } from 'uuid';
//import { setInterval } from 'timers';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})


export class VideoComponent implements OnInit, OnDestroy
{
  player: YT.Player;
  private id: string;
  ipAdress: any;
  idSession: string;
  intervalId: any;
  isVideoPlay: boolean;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private sessiondata: SessiondataService,
    private http: HttpClient){
    this.http.get('https://jsonip.com').subscribe((ipOfNetwork) => this.ipAdress = ipOfNetwork['ip']);
  }
  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.idSession = uuid();
    this.isVideoPlay = false;
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
      this.isVideoPlay = true;
      this.intervalId = setInterval(this.sendPing, 30000);
    }
  }

  sendPing() {
    console.log("Update iz sendPing: " + this.getPlayerStatus);
    this.sessiondata.update({
      Id: this.idSession,
      Status: this.getPlayerStatus,
      UserAdress: this.ipAdress,
      IdVideo: this.id
    }, this.idSession);
  }

  getPlayerStatus(): string {
    console.log("Poziv funkcije: " + this.player.getPlayerState());
    const playerStatus = this.player.getPlayerState();
    if (playerStatus == 0) {
      return 'ENDED';
    }
    else if (playerStatus == 1) {
      return 'PLAY';
    }
    else if (playerStatus == 2) {
      return 'PAUSE';
    }
    else if (playerStatus == 3) {
      return 'BUFFERING';
    }
    else if (playerStatus == 5) {
      return 'VIDEO_CUED';
    }
  }

  ngOnDestroy() {
    if (this.isVideoPlay == true) {
      console.log("Status:", "FINISHED");
      this.sessiondata.update({
        Id: this.idSession,
        Status: 'FINISHED',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      }, this.idSession);
      clearInterval(this.intervalId);
    }
  }
}


