import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
  
  @Input() defaultImage: string | undefined = undefined;
  @Input() showUpload: boolean = true;
  @Input() source: string | undefined;
  // bad
  @Input() download!: Observable<FileResponse>;
  @Input() upload!: ((fileUpload: FileUpload) => Observable<any>) | undefined;

  @Output() onUploaded = new EventEmitter<string>();
  
  constructor(public domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.defaultImage = this.defaultImage || this.DEFAULT_IMAGE;
    if (this.source && this.source != this.defaultImage && this.download) {
      this.download
        .pipe(
          takeUntil(this.unsubscribe),
          map(fileResponse => this.createImageFromBlob(fileResponse.data))
        )
        .subscribe();
    }
    else {
      this.dataSource = this.defaultImage;
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

        if (this.upload) {
          this.upload(upload)
            .pipe(takeUntil(this.unsubscribe))
            .subscribe();
        }
        
        this.onUploaded.emit(base64result);
        
      }
    }
  }

  private createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () =>
    {
      debugger;
      const base64Image = this.domSanitizer.bypassSecurityTrustUrl(reader.result as string);
      this.dataSource = (base64Image as string)
    }, false);
 
    if (image) {
       reader.readAsDataURL(image);
    }
  }
}
