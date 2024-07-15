import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.scss'
})
export class PerfilComponent implements OnInit {
  imageSrc: string | undefined;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadImage();
  }

  loadImage(): void {
    this.http.get<any>('https://avatars.githubusercontent.com/u/55888983?v=4').subscribe(data => {
      this.imageSrc = data.imageUrl;
    });
  }
}
