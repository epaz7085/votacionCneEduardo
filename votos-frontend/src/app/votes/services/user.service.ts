import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id: string;
  fullName: string;
  email: string;
  role: string;
  hasVoted: boolean;
  votedForName?: string;
  voteTimestamp?: string;
}

@Injectable({ providedIn: 'root' })
export class UserService {

  private apiUrl = 'http://localhost:5087/api/users';

  constructor(private http: HttpClient) {}

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }
}
