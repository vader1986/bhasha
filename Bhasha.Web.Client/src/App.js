import React from 'react';
import './App.css';
import Container from '@material-ui/core/Container';
import { Paper } from '@material-ui/core';
import ProfileSelection from './features/ProfileSelection';
import ChapterSelection from './features/ChapterSelection';
import Chapter from './features/Chapter';
import { DISPLAY_CHAPTER, DISPLAY_CHAPTER_SELECTION, DISPLAY_PROFILE_SELECTION } from './consts';
import NavigationBar from './features/NavigationBar';

function App() {
  const [profile, setProfile] = React.useState(undefined);
  const [chapter, setChapter] = React.useState(undefined);
  const [completedIds, setCompletedIds] = React.useState([])
  const [screen, setScreen] = React.useState(DISPLAY_PROFILE_SELECTION);

  const onProfileUpdate = profile => {
    setProfile(profile);
  };

  const onSelectProfile = profile => {
    setProfile(profile);
    setScreen(DISPLAY_CHAPTER_SELECTION);
  };

  const onSelectChapter = chapter => {
    setChapter(chapter);
    setScreen(DISPLAY_CHAPTER);
  };

  const onChapterCompleted = chapter => {
    setCompletedIds(previous => previous.concat([chapter.id]));
    setChapter(undefined);
    setScreen(DISPLAY_CHAPTER_SELECTION);
  };

  const onSwitchToProfileSelection = () => {
    setProfile(undefined);
    setChapter(undefined);
    setScreen(DISPLAY_PROFILE_SELECTION);
  };

  const onSwitchToChapterSelection = () => {
    setChapter(undefined);
    setScreen(DISPLAY_CHAPTER_SELECTION);
  };

  const renderScreen = () => {
    switch (screen)
    {
      case DISPLAY_PROFILE_SELECTION:
        return <ProfileSelection
                  onSelect={onSelectProfile} />;

      case DISPLAY_CHAPTER_SELECTION:
        return <ChapterSelection
                  onSelect={onSelectChapter}
                  completedIds={completedIds}
                  profile={profile} />;

      case DISPLAY_CHAPTER:
        return <Chapter
                  chapter={chapter}
                  profile={profile}
                  onCompleted={onChapterCompleted}
                  onProfileUpdate={onProfileUpdate} />;

      default:
        return <ProfileSelection
                  onSelect={onSelectProfile} />;
    }
  };

  return (
    <Container maxWidth="sm" className="App">
      <NavigationBar
        screen={screen}
        profile={profile}
        chapter={chapter}
        onSelectProfile={onSwitchToProfileSelection}
        onSelectChapter={onSwitchToChapterSelection} />
      <Paper>
        {renderScreen()}
      </Paper>
    </Container>
  );
}

export default App;