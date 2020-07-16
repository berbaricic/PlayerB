import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoComponent } from './video.component';
import { RouterModule } from '@angular/router';
import { YouTubePlayerModule } from '@angular/youtube-player';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [VideoComponent],
  exports: [VideoComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: 'video/:id', component: VideoComponent},
    ]),
    YouTubePlayerModule,
    HttpClientModule
  ]
})
export class VideoModule { }
