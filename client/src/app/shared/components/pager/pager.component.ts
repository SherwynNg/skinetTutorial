import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-pager',
  standalone: true,
  imports: [PaginationModule],
  templateUrl: './pager.component.html',
  styleUrl: './pager.component.scss',
})
export class PagerComponent implements OnInit {
  @Input() totalCount: number = 0;
  @Input() pageSize: number = 0;
  @Output() pageChanged = new EventEmitter<number>();

  ngOnInit(): void {}

  onPagerChanged(event: any) {
    this.pageChanged.emit(event.page);
  }
}
