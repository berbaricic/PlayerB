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
      console.log("Status:", "UNSTARTED");
      this.sessiondata.create({
        Id: this.idSession,
        Status: 'UNSTARTED',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
      this.isVideoPlay = true;
      this.intervalId = setInterval(() => this.sendPing(), 30000);
    }
    else if (event.data == 0) {
      this.playerState = 'ENDED';
    }
    else if (event.data == 1) {
      this.playerState = 'PLAY';
    }
    else if (event.data == 2) {
      this.playerState = 'PAUSE';
    }
    else if (event.data == 3) {
      this.playerState = 'BUFFERING';
    }
    else if (event.data == 5) {
      this.playerState = 'VIDEO_CUED';
    }
  }

  sendPing() {
    this.sessiondata.update({
      Id: this.idSession,
      Status: this.playerState,
      UserAdress: this.ipAdress,
      IdVideo: this.id
    }, this.idSession);
  }

  //getPlayerStatus(): any {
  //  this.playerState = 1;
  //  console.log("State fiksni: " + this.playerState);
  //  console.log("Instanca: ", this.player);
  //  if (this.playerState == 0) {
  //    return 'ENDED';
  //  }
  //  else if (this.playerState == 1) {
  //    return 'PLAY';
  //  }
  //  else if (this.playerState == 2) {
  //    return 'PAUSE';
  //  }
  //  else if (this.playerState == 3) {
  //    return 'BUFFERING';
  //  }
  //  else if (this.playerState == 5) {
  //    return 'VIDEO_CUED';
  //  }
  //}

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


