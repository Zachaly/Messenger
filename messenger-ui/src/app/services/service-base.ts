import { HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";

export default class ServiceBase {
    private token = () => this.authService.currentUser.authToken

    httpHeaders = () =>
        new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${this.token()}`
        })

    authorizeHeader = () =>
        new HttpHeaders({
            'Authorization': `Bearer ${this.token()}`
        })

    constructor(private authService: AuthService) {

    }
}