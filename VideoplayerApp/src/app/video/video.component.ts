import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, Event, NavigationEnd } from '@angular/router';
import { SessiondataService } from './sessiondata.service';
import { HttpClient } from '@angular/common/http';
import { v4 as uuid } from 'uuid';

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
  playerState: any;


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
      console.log("Status:", "START");
      this.playerState = 'START';
      this.sessiondata.create({
        Id: this.idSession,
        Status: this.playerState,
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
      this.isVideoPlay = true;
      this.intervalId = setInterval(() => this.sendPing(), 30000);
    }
    else if (event.data == 0) {
      this.playerState = 'ENDED';
      this.sessiondata.create({
        Id: this.idSession,
        Status: this.playerState,
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
    }
    else if (event.data == 1) {
      this.playerState = 'PLAY';
      this.sessiondata.create({
        Id: this.idSession,
        Status: this.playerState,
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
    }
    else if (event.data == 2) {
      this.playerState = 'PAUSE';
      this.sessiondata.create({
        Id: this.idSession,
        Status: this.playerState,
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
    }
    else if (event.data == 5) {
      this.playerState = 'VIDEO_CUED';
      this.sessiondata.create({
        Id: this.idSession,
        Status: this.playerState,
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
    }
  }

  sendPing() {
    this.playerState = 'PING';
    this.sessiondata.create({
      Id: this.idSession,
      Status: this.playerState,
      UserAdress: this.ipAdress,
      IdVideo: this.id
    });
  }

  ngOnDestroy() {
    if (this.isVideoPlay == true) {
      this.playerState = 'CLOSED';
      this.sessiondata.create({
        Id: this.idSession,
        Status: this.playerState,
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
      clearInterval(this.intervalId);
    }
  }
}


