import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { IProduct } from './models/product';
import { IPagingation } from './models/pagination';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavBarComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'SkiNet';
  products: IProduct[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any>('http://localhost:5165/api/products').subscribe(
      (response: IPagingation) => {
        this.products = response.data;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
