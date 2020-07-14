import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoComponent } from './video.component';
import { RouterModule } from '@angular/router';
import { YouTubePlayerModule } from '@angular/youtube-player';


@NgModule({
  declarations: [VideoComponent],
  exports: [VideoComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: 'video/:id', component: VideoComponent},
    ]),
    YouTubePlayerModule
  ]
})
export class VideoModule { }
