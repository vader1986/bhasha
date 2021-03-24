export function getLanguageKey(language) {
    return language.region !== undefined && 
           language.region !== null ? language.id + "_" + language.region : language.id;
}