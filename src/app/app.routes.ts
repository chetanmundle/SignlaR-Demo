import { Routes } from '@angular/router';
import { LoginComponent } from './pages/auth/login/login.component';
import { RegisterComponent } from './pages/auth/register/register.component';
import { HomeComponent } from './pages/org/home/home.component';
import { authGuard } from './core/Guards/auth.guard';
import { CreateConversationComponent } from './pages/org/create-conversation/create-conversation.component';

export const routes: Routes = [
    {
        path : '',
        redirectTo : 'Home',
        pathMatch : 'full'
    },
    {
        path : 'Login',
        component : LoginComponent,
        title : 'Login'
    },
    {
        path : 'Register',
        component : RegisterComponent,
        title : 'Register'
    },
    {
        path : 'Home',
        component :HomeComponent,
        title : "Home",
        canActivate: [authGuard],
        data: { roles: ['User'] },
    },
    {
        path : "Create-Conversation",
        component : CreateConversationComponent,
        title : "Create-Conversation",
        canActivate : [authGuard],
        data: { roles: ['User'] },
    }
];
