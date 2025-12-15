import { Injectable } from '@angular/core';
import {
  Firestore,
  collection,
  getDocs,
  query,
  where,
  orderBy
} from '@angular/fire/firestore';
import { Observable, from } from 'rxjs';
import { map } from 'rxjs/operators';

export interface Notification {
  id: string;
  title: string;
  message: string;
  createdAt: any;
  read: boolean;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {

  constructor(private firestore: Firestore) {}

  getAdminNotifications(): Observable<Notification[]> {

    const ref = collection(this.firestore, 'notifications');

    const q = query(
      ref,
      where('role', '==', 'admin'),
      orderBy('createdAt', 'desc')
    );

    return from(getDocs(q)).pipe(
      map(snapshot =>
        snapshot.docs.map(doc => ({
          id: doc.id,
          ...(doc.data() as any)
        }))
      )
    );
  }
}
