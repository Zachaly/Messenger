import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

const API_URL = 'https://localhost:5001/api/image'

@Injectable({
  providedIn: 'root'
})
export class ImageService extends ServiceBase {

  constructor(private http: HttpClient, authService: AuthService) {
    super(authService)
  }

  getUrl(type: string, id: number): string {
    return `${API_URL}/${type}/${id}`
  }

  uploadDirectMessageImages(files: FileList, messageId: number): Observable<any> {
    const formData = new FormData()

    formData.append('MessageId', messageId.toString())

    for (let i = 0; i < files.length; i++) {
      formData.append("Files", files.item(i)!)
    }

    return this.http.post(`${API_URL}/direct-message`, formData, {
      headers: this.authorizeHeader()
    })
  }

  uploadProfileImage(userId: number, file?: File | null): Observable<any> {
    const formData = new FormData()

    if (file) {
      formData.append('File', file)
    }

    formData.append('UserId', userId.toString())

    return this.http.put(`${API_URL}/profile`, formData, { headers: this.authorizeHeader() })
  }
}
