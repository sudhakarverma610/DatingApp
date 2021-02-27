import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Photo } from 'src/app/models/Photo';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/Services/Auth.service';
import { FileUploader } from 'ng2-file-upload';
import { UserService } from 'src/app/Services/user.service';
import { AlertifyService } from 'src/app/Services/Alertify.service';

@Component({
  selector: 'app-photoEditor',
  templateUrl: './photoEditor.component.html',
  styleUrls: ['./photoEditor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  @Input() photos: Photo[];
  @Output() mainPhotoChanged = new EventEmitter<string>();
  baseUrl = environment.baseUrl;
  currentMain: Photo;
  constructor(private authService: AuthService,
              private userService: UserService,
              private alert: AlertifyService) { }
  ngOnInit() {
    this.initializeUploader();
  }
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
    initializeUploader(){
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024

    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response){
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
      }
    };
  }
  SetPhotoAsMain(photo: Photo){
    this.userService.SetPhotoAsMain(photo.id, this.authService.decodedToken.nameid).subscribe(res => {
      console.log('Successfully set as Main');
      /** setting current photo as false  */
      this.photos.filter(x => x.isMain === true)[0].isMain = false;
      this.photos.filter(x => x.id === photo.id)[0].isMain = true;
      this.authService.photoUrl.next(photo.url);
      localStorage.setItem('photoUrl', photo.url);
      this.mainPhotoChanged.emit(photo.url);
    },
    error => {
      this.alert.Error(error);
    });
  }
}
