import React from 'react';
import './App.css';
import Container from '@material-ui/core/Container';
import { Paper } from '@material-ui/core';
import ProfileList from './features/ProfileList';

function App() {
  return (
    <Container maxWidth="sm" className="App">
      <Paper>
        <ProfileList />
      </Paper>
    </Container>
  );
}
export default App;