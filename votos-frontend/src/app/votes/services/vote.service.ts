import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VoteService {

  private apiUrl = 'http://localhost:5087/api/Votes';

  constructor(private http: HttpClient) {}

  getVoteStatus(): Observable<any> {
    return this.http.get(`${this.apiUrl}/status`);
  }

  vote(candidateId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${candidateId}`, {});
  }
}
