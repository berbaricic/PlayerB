import { Component, OnInit, Input } from '@angular/core';
import { VideoDetail } from 'src/app/search/details.model';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent implements OnInit {
  @Input() result: VideoDetail;
  constructor() { }

  ngOnInit() {
  }

}
