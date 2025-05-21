<script setup>
import { ref, onMounted } from 'vue'
import { getAllPatients } from '../services/patientService'

const patients = ref([])

onMounted(async () => {
  try {
    const response = await getAllPatients()
    patients.value = response.data
  } catch (error) {
    console.error('Failed to fetch patients:', error)
  }
})
</script>

<template>
  <div>
    <h1>Patients</h1>
    <ul>
      <li v-for="patient in patients" :key="patient.id">
        {{ patient.firstName }} {{ patient.lastName }}
      </li>
    </ul>
  </div>
</template>
