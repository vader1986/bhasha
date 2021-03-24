import { Button, Chip, Paper } from '@material-ui/core';
import axios from 'axios';
import React, { useEffect } from 'react';
import { getLanguageKey } from '../utils';

const isReady = (from, to, profiles) => {
    return from !== undefined && to !== undefined && from !== to &&
        !profiles.some(x => 
            getLanguageKey(x.from) === getLanguageKey(from) && 
            getLanguageKey(x.to) === getLanguageKey(to));
};

function ProfileCreateDialog(props) {

    const [languages, setLanguages] = React.useState([]);
    const [selectedFrom, setSelectedFrom] = React.useState(undefined);
    const [selectedTo, setSelectedTo] = React.useState(undefined);

    useEffect(() => {
        axios
          .get(`api/profile/languages`)
          .then(res => setLanguages(res.data));
    }, [setLanguages]);

    const onClick = () => {
        const from = getLanguageKey(selectedFrom);
        const to = getLanguageKey(selectedTo);

        axios
          .post(`api/profile/create?from=${from}&to=${to}`)
          .then(res => props.onCreate(res.data));

        setSelectedFrom(undefined);
        setSelectedTo(undefined);
    };

    const onSelect = (previous, next) => () => {
        if (selectedFrom === previous) {
            setSelectedFrom(next);
        }
        else
        if (selectedTo === previous) {
            setSelectedTo(next);
        }
    };

    return (
        <div>
            <Paper>
                {languages.map(language => 
                <li key={getLanguageKey(language)}>
                    <Chip
                        label={language.name}
                        onClick={onSelect(undefined, language)}
                    />
                </li>
                )}
                { selectedFrom !== undefined && 
                    <div>
                        FROM
                        <Chip
                            label={selectedFrom.name}
                            onDelete={onSelect(selectedFrom, undefined)} />
                    </div> }
                { selectedTo !== undefined && 
                    <div>TO 
                        <Chip
                            label={selectedTo.name}
                            onDelete={onSelect(selectedTo, undefined)} />
                    </div> }
                { isReady(selectedFrom, selectedTo, props.profiles) && 
                    <Button
                        variant="contained" 
                        onClick={onClick}>Create</Button>}
            </Paper>            
        </div>
    );
}

export default ProfileCreateDialog;