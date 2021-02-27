import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/Services/user.service';
import { AlertifyService } from 'src/app/Services/Alertify.service';
import { ActivatedRoute } from '@angular/router';
import {NgxGalleryOptions} from '@kolkov/ngx-gallery';
import {NgxGalleryImage} from '@kolkov/ngx-gallery';
import {NgxGalleryAnimation} from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit {
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(private userService: UserService,
              private alertifyService: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {

    this.route.data.subscribe(data => {
      this.user = data.user;
    });
    this.galleryOptions = [
      {
        width: '600px',
        height: '400px',
        thumbnailsColumns: 4,
        arrowPrevIcon: 'fa fa-chevron-left',
        arrowNextIcon: 'fa fa-chevron-right',
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        height: '600px',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];

    this.galleryImages = [
      {
        small: 'https://preview.ibb.co/jrsA6R/img12.jpg',
        medium: 'https://preview.ibb.co/jrsA6R/img12.jpg',
        big: 'https://preview.ibb.co/jrsA6R/img12.jpg'
      },
      {
        small: 'https://preview.ibb.co/kPE1D6/clouds.jpg',
        medium: 'https://preview.ibb.co/kPE1D6/clouds.jpg',
        big: 'https://preview.ibb.co/kPE1D6/clouds.jpg'
      },
      {
        small: 'https://preview.ibb.co/mwsA6R/img7.jpg',
        medium: 'https://preview.ibb.co/mwsA6R/img7.jpg',
        big: 'https://preview.ibb.co/mwsA6R/img7.jpg'
      }, {
        small: 'https://preview.ibb.co/kZGsLm/img8.jpg',
        medium: 'https://preview.ibb.co/kZGsLm/img8.jpg',
        big: 'https://preview.ibb.co/kZGsLm/img8.jpg'
      },
    ];
    this.galleryImages =  this.getImages();

  }
  getImages(): NgxGalleryImage[]{
    const images = new Array<NgxGalleryImage>();
    for (const photo of this.user.photos){
    const image =  new NgxGalleryImage({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description
      });
    images.push(image);
    }
    return images;
  }

}
