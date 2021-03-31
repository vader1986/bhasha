import { setup } from 'axios-cache-adapter'

export const api = setup({
  cache: {
    maxAge: 120 * 60 * 1000, // 2 hours
    exclude: {
        query: false
    }
  }
});

export function getLanguageKey(language) {
    return language.region !== undefined && 
           language.region !== null ? language.id + "_" + language.region : language.id;
};