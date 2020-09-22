import { Component, ChangeDetectionStrategy } from '@angular/core';
import { VideoDetail } from 'src/app/search/details.model';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent {
  results: VideoDetail[] = [];
  loading: boolean;
  message = '';

  private resultState = new BehaviorSubject<VideoDetail[]>(this.results);

  public videoDetail$ = this.resultState.asObservable();

  updateResults(results: VideoDetail[]): void {
    this.resultState.next(results);
    this.results = results;
    if (this.results.length === 0) {
      this.message = 'Not found...';
    } else {
      this.message = 'Top 10 results:';
    }
  }
}
