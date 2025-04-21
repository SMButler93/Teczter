import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ITest } from "../Interfaces/test-interface";
import { environment } from "../../../environments/environment.local";

@Injectable({
    providedIn: "root"
})
export class TestService {
    private baseUrl = environment.baseUrl;

    constructor(private http: HttpClient) {}

    getTestById(id: number): Observable<ITest> {
        return this.http.get<ITest>(`${this.baseUrl}/Test/${id}`)
    }

    getTestSearchResults(pageNumber?: number, testName?: string, owningDepartment? : string) {
        return this.http.get<ITest[]>(`${this.baseUrl}/Test`);
    }
}