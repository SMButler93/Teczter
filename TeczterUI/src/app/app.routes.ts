import { Routes } from '@angular/router';
import { TestComponent } from './Tests/test/test.component';

export const routes: Routes = [
    {
        path: 'Test', 
        loadComponent: () => import('./Tests/test/test.component').then(x => x.TestComponent)
    }
];
