import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})
export class VideoComponent implements OnInit {

  player: YT.Player;
  private id: string;

  constructor(private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
  }

  savePlayer(player) {
    this.player = player;
    console.log("player instance", player);
  }

  onStateChange(event) {
    if (event.data == -1) {
      console.log("Status:", "UNSTARTED");
    }
    else if (event.data == 0) {
      console.log("Status:", "ENDED");
    }
    else if (event.data == 1) {
      console.log("Status:", "PLAYING");
    }
    else if (event.data == 2) {
      console.log("Status:", "PAUSED");
    }
  }
}


