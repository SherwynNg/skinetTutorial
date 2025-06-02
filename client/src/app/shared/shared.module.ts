import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagerComponent } from './components/pager/pager.component';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  exports: [PaginationModule],
})
export class SharedModule {}
