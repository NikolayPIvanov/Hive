import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { FileResponse, FileUpload } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.scss']
})
export class ImageUploadComponent implements OnInit {
  private DEFAULT_IMAGE = '/assets/user.png';
  private unsubscribe = new Subject();

  public dataSource: string | undefined = undefined;
  
  @Input() source: string | undefined;
  // bad
  @Input() download!: Observable<FileResponse>;
  @Input() upload!: (fileUpload: FileUpload) => Observable<any>;
  
  constructor(public domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    debugger;
    if (this.source && this.source != this.DEFAULT_IMAGE && this.download) {
      this.download
        .pipe(
          takeUntil(this.unsubscribe),
          map(fileResponse => this.createImageFromBlob(fileResponse.data))
        )
        .subscribe();
    }
    else {
      this.dataSource = this.DEFAULT_IMAGE;
    }
  }

  onFileSelected(event: any) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.dataSource = <string>event.target!.result;
        var base64result = this.dataSource.split(',')[1];
        const upload = FileUpload.fromJS({ fileData: base64result });

        this.upload(upload)
          .pipe(takeUntil(this.unsubscribe))
          .subscribe();
      }
    }
  }

  private createImageFromBlob(image: Blob) {
    debugger;
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      const base64Image = this.domSanitizer.bypassSecurityTrustUrl(reader.result as string);
      this.dataSource = (base64Image as string)
    }, false);
 
    if (image) {
       reader.readAsDataURL(image);
    }
  }
}
