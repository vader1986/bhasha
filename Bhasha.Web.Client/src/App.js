import React from 'react';
import './App.css';
import Container from '@material-ui/core/Container';
import { Paper } from '@material-ui/core';
import ProfileSelection from './features/ProfileSelection';
import ChapterSelection from './features/ChapterSelection';
import Chapter from './features/Chapter';
import { DISPLAY_CHAPTER, DISPLAY_CHAPTER_SELECTION, DISPLAY_PROFILE_SELECTION } from './consts';

function App() {
  const [profile, setProfile] = React.useState(undefined);
  const [chapter, setChapter] = React.useState(undefined);
  const [screen, setScreen] = React.useState(undefined);

  const onSelectProfile = profile => {
    setProfile(profile);
    setScreen(DISPLAY_CHAPTER_SELECTION);
  };

  const onSelectChapter = chapter => {
    setChapter(chapter);
    setScreen(DISPLAY_CHAPTER);
  };

  const renderScreen = () => {
    switch (screen)
    {
      case DISPLAY_PROFILE_SELECTION:
        return <ProfileSelection onSelect={onSelectProfile} />;

      case DISPLAY_CHAPTER_SELECTION:
        return <ChapterSelection onSelect={onSelectChapter} profile={profile}/>;

      case DISPLAY_CHAPTER:
        return <Chapter chapter={chapter} />;

      default:
        return <ProfileSelection onSelect={onSelectProfile} />;
    }
  };

  return (
    <Container maxWidth="sm" className="App">
      <Paper>
        {renderScreen()}
      </Paper>
    </Container>
  );
}

export default App;