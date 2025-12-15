import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Candidate {
  id: string;
  name: string;
  party: string;
  photoUrl: string;
  logoUrl: string;
  proposals: string[];
  votesCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class CandidateService {

  private apiUrl = 'http://localhost:5087/api/candidates';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Candidate[]> {
    return this.http.get<Candidate[]>(this.apiUrl);
  }

  create(candidate: any): Observable<Candidate> {
    return this.http.post<Candidate>(this.apiUrl, candidate);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
