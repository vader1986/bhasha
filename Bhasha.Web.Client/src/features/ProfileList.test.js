import { render, screen } from '@testing-library/react';
import ProfileList from './ProfileList';

const testProfiles = [
  {
    id: "4b970432-25cf-429a-b8c5-8691d120bb98",
    userId: "user-111",
    native: {
      id: "en",
      region: "UK",
      name: "English"
    },
    target: {
      id: "bn",
      region: null,
      name: "Bengali"
    },
    level: 2,
    completedChapters: 20
  },
  {
    id: "47c74ca7-cb82-4063-b55e-829051894326",
    userId: "user-222",
    native: {
      id: "bn",
      region: null,
      name: "Bengali"
    },
    target: {
      id: "en",
      region: "UK",
      name: "English"
    },
    level: 10,
    completedChapters: 50
  }
];

test('renders multiple profiles', async () => {
  render(<ProfileList profiles={testProfiles} />);

  const firstProfile = await screen.findByText(/English - Bengali/i);
  expect(firstProfile).toBeDefined();

  const secondProfile = await screen.findByText(/Bengali - English/i);
  expect(secondProfile).toBeDefined();
});
