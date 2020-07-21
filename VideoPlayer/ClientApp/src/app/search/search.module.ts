import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchBoxComponent } from 'src/app/search/search-box/search-box.component';
import { SearchResultComponent } from 'src/app/search/search-result/search-result.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [SearchBoxComponent, SearchResultComponent],
  exports: [SearchBoxComponent, SearchResultComponent],
  imports: [
    CommonModule,
    HttpClientModule
  ]
})
export class SearchModule { }
