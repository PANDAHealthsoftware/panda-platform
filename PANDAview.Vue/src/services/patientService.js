import api from './api';

export const getAllPatients = () => api.get('/patients');
export const getPatient = (id) => api.get(`/patients/${id}`);
export const createPatient = (patient) => api.post('/patients', patient);
export const updatePatient = (id, patient) => api.put(`/patients/${id}`, patient);
export const deletePatient = (id) => api.delete(`/patients/${id}`);
