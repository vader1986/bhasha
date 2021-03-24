import { render, screen } from '@testing-library/react';
import ProfileList from './ProfileList';

const testProfiles = [
  {
    id: "4b970432-25cf-429a-b8c5-8691d120bb98",
    userId: "user-111",
    from: {
      id: "en",
      region: "UK",
      name: "English"
    },
    to: {
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
    from: {
      id: "bn",
      region: null,
      name: "Bengali"
    },
    to: {
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
