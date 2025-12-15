import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http'; // ¡Necesario!

import { routes } from './app.routes';
import { authInterceptor } from './votes/interceptors/auth.interceptor';

import { initializeApp, provideFirebaseApp } from '@angular/fire/app';
import { provideFirestore, getFirestore } from '@angular/fire/firestore';

const firebaseConfig = {
  apiKey: "AIzaSyDtHDveBjWBSz-mIs6AUetMQyeHeLS0bTM",
  authDomain: "proyectovotacion-7f1b2.firebaseapp.com",
  projectId: "proyectovotacion-7f1b2",
  storageBucket: "proyectovotacion-7f1b2.firebasestorage.app",
  messagingSenderId: "820764991499",
  appId: "1:820764991499:web:402cccb51fac052eb3270e"
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),

    provideHttpClient(
      withInterceptors([authInterceptor])
    ),

    provideFirebaseApp(() => initializeApp(firebaseConfig)),
    provideFirestore(() => getFirestore())
  ]
};

