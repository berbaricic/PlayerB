import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchBoxComponent } from 'src/app/search/search-box/search-box.component';
import { SearchResultComponent } from 'src/app/search/search-result/search-result.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [SearchBoxComponent, SearchResultComponent],
  exports: [SearchBoxComponent, SearchResultComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    RouterModule
  ]
})
export class SearchModule { }
