import { CommonModule } from '@angular/common';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-paging-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './paging-header.component.html',
  styleUrl: './paging-header.component.scss',
})
export class PagingHeaderComponent implements OnInit {
  @Input() pageNumber!: number;
  @Input() pageSize!: number;
  @Input() totalCount!: number;

  ngOnInit(): void {}
}
