import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  ipAdress: any;
  idSession: string;
  //timeRequest: number;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private sessiondata: SessiondataService,
    private http: HttpClient){
    this.http.get('https://jsonip.com').subscribe((ipOfNetwork) => this.ipAdress = ipOfNetwork['ip']);
  }
  ngOnInit(){
    this.id = this.route.snapshot.paramMap.get('id');
    this.idSession = uuid();
    //this.timeRequest = new Date().getTime();
  }

  savePlayer(player){
    this.player = player;
    console.log("player instance", player);
  }

  onStateChange(event){
    if (event.data == -1) {
      console.log("Status:", "UNSTARTED");
    }
    else if (event.data == 0) {
      console.log("Status:", "ENDED");
    }
    else if (event.data == 1) {
      console.log("Status:", "PLAYING");
      this.sessiondata.create({
        Id: this.idSession,
        Status: 'PLAY',
        UserAdress: this.ipAdress,
        IdVideo: this.id
      });
    }
    else if (event.data == 2) {
      console.log("Status:", "PAUSED");
    }
    else if (event.data == 3) {
      console.log("Status:", "BUFFERING");
    }
    else if (event.data == 5) {
      console.log("Status:", "VIDEO_CUED");
    }
  }
}


