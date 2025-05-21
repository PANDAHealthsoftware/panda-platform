<script setup>
</script>

<template>
  <nav>
    <router-link to="/">Home</router-link> |
    <router-link to="/patients">Patients</router-link> |
    <router-link to="/appointments">Appointments</router-link>
  </nav>

  <!-- Dynamic route outlet -->
  <router-view />
</template>

<style scoped>
nav {
  padding: 1rem;
  background-color: #f9f9f9;
  border-bottom: 1px solid #ddd;
}
router-link {
  margin-right: 1rem;
  text-decoration: none;
  color: #42b983;
}
router-link.active {
  font-weight: bold;
  color: #35495e;
}
</style>
