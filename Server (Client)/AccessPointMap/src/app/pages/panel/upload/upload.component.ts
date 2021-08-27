import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  public defaultUploadForm: FormGroup;
  public wigleUploadForm: FormGroup;
  public aircrackngUploadForm: FormGroup;

  constructor(private accessPointService: AccessPointService) { }

  ngOnInit(): void {
    this.initializeForms();
  }

  private initializeForms(): void {
    this.defaultUploadForm = new FormGroup({
      content: new FormControl('', [ Validators.required ])
    });

    this.wigleUploadForm = new FormGroup({
      file: new FormControl('', [ Validators.required ])
    });

    this.aircrackngUploadForm = new FormGroup({
      file: new FormControl('', [ Validators.required ])
    });
  }

  public setWigleFileEvent(e: any): void {
    if (e.target.files.length > 0) {
      const file = e.target.files[0] as File;
      this.wigleUploadForm.get('file').setValue(file);
    }
  }

  public setAircrackngFileEvent(e: any): void {
    if (e.target.files.length > 0) {
      const file = e.target.files[0] as File;
      this.aircrackngUploadForm.get('file').setValue(file);
    }
  }

  public uploadDefault(): void {
    if(this.defaultUploadForm.valid) {
      const accessPoints = JSON.parse(this.defaultUploadForm.get('content').value) as Array<AccessPoint>;

      this.accessPointService.postMany(accessPoints, 1).subscribe(() => {
        this.defaultUploadForm.reset();
      },
      (error) => {
        console.error(error);
      });
    }
  }

  public uploadWigle(): void {
    if(this.wigleUploadForm.valid) {
      const file = this.wigleUploadForm.get('file').value as File;

      this.accessPointService.postManyWigle(file, 1).subscribe(() => {
        this.wigleUploadForm.reset();
      },
      (error) => {
        console.error(error);
      })
    }
  }

  public uploadAircrackng(): void {
    if(this.aircrackngUploadForm.valid) {
      const file = this.aircrackngUploadForm.get('file').value as File;

      this.accessPointService.postManyAircrackng(file, 1).subscribe(() => {
        this.aircrackngUploadForm.reset();
      },
      (error) => {
        console.error(error);
      })
    }
  }

}
