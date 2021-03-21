import React from 'react';
import './App.css';
import Container from '@material-ui/core/Container';
import { Paper } from '@material-ui/core';
import ProfileList from './features/ProfileList';

const fakeProfile = {
  id: "d494e433-ee72-48e8-a45a-3f0eed6a0d34",
  userId: "test",
  from: {
    id: "en",
    name: "English",
    region: "UK"
  },
  to: {
    id: "bn",
    name: "Bengali",
    region: null
  },
  level: 1,
  completedChapters: 0
};

const fakeProfile2 = {
  id: "d494e433-ee72-48e8-a45a-3f0eed6a0d34",
  userId: "test",
  from: {
    id: "bn",
    name: "Bengali",
    region: null
  },
  to: {
    id: "en",
    name: "English",
    region: "UK"
  },
  level: 1,
  completedChapters: 0
};

function App() {
  return (
    <Container maxWidth="sm" className="App">
      <Paper>
        <ProfileList profiles={[fakeProfile, fakeProfile2]} />
      </Paper>
    </Container>
  );
}
export default App;