import React from 'react';
import './App.css';
import Container from '@material-ui/core/Container';
import { Paper } from '@material-ui/core';
import ProfileSelection from './features/ProfileSelection';

function App() {
  return (
    <Container maxWidth="sm" className="App">
      <Paper>
        <ProfileSelection />
      </Paper>
    </Container>
  );
}
export default App;