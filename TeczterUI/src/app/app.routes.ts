import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'Teczter/Test/:testId', 
        loadComponent: () => import('./Tests/test/test.component').then(x => x.TestComponent)
    }
];
