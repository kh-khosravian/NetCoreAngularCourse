import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { PhotoModel } from 'src/app/_model/photoModel';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/User.service';
import { AlertifyService } from 'src/app/_services/AlertifyService.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Output() GetMemberPhotoChange = new EventEmitter<string>();
  @Input() photos: PhotoModel[];
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  response: string;
  baseUrl = environment.apiUrl;
  MainPhoto: PhotoModel;

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) {
    this.initialzeUploader();
  }

  ngOnInit() {
  }

  initialzeUploader = () => {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodeToken.nameid + '/photos/',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, header) => {
      if (response) {
        const res: PhotoModel = JSON.parse(response);
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

  setMainPhoto = (photo: PhotoModel) => {
    this.userService.setMainPhoto(this.authService.decodeToken.nameid, photo.id).subscribe(() => {
      this.MainPhoto = this.photos.filter(s => s.isMain === true)[0];
      this.MainPhoto.isMain = false;
      photo.isMain = true;
      this.GetMemberPhotoChange.emit(photo.url);
      this.authService.currentUser.photoURL = photo.url;
      localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
    }, err => {
      this.alertify.Error(err);
    });
  }

  DeletePhoto = (photo: PhotoModel) => {
    this.alertify.confirm('Are you sure?', () => {
      this.userService.DeletePhoto(this.authService.currentUser.id, photo.id).subscribe(() => {
        this.photos = this.photos.filter(p => p !== photo);
        this.alertify.success('Image deleted successfully.');
      }, e => {
        this.alertify.Error(e);
      });
    });
  }
}
