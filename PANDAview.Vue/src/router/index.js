import { createRouter, createWebHistory } from 'vue-router';

import Home from '../views/Home.vue';
import PatientAdmin from '../views/PatientAdmin.vue';
import AppointmentAdmin from '../views/AppointmentAdmin.vue';

const routes = [
    { path: '/', component: Home },
    { path: '/patients', component: PatientAdmin },
    { path: '/appointments', component: AppointmentAdmin },
];

export const router = createRouter({
    history: createWebHistory(),
    routes,
});
