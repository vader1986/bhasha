import React from 'react';
import './App.css';
import Container from '@material-ui/core/Container';
import { Paper } from '@material-ui/core';
import ProfileSelection from './features/ProfileSelection';
import ChapterSelection from './features/ChapterSelection';

function App() {
  
  const [profile, setProfile] = React.useState(undefined);

  const onSelectProfile = (profile) => {
    setProfile(profile);
  };

  return (
    <Container maxWidth="sm" className="App">
      <Paper>
        { profile === undefined && <ProfileSelection onSelect={onSelectProfile} />}
        { profile !== undefined && <ChapterSelection profile={profile}/>}
      </Paper>
    </Container>
  );
}
export default App;