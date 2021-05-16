import { Button, Chip, makeStyles, Paper } from '@material-ui/core';
import React, { useEffect } from 'react';
import { api, getLanguageKey } from '../utils';

const useStyles = makeStyles((theme) => ({
    container: {
      display: 'flex',
      justifyContent: 'center',
      flexWrap: 'wrap',
      '& > *': {
        margin: theme.spacing(0.5),
      },
    },
  }));

const isReady = (from, to, profiles) => {
    return from !== undefined && to !== undefined && from !== to &&
        !profiles.some(x => 
            getLanguageKey(x.from) === getLanguageKey(from) && 
            getLanguageKey(x.to) === getLanguageKey(to));
};

function ProfileCreateDialog(props) {
    const classes = useStyles();
    const [languages, setLanguages] = React.useState([]);
    const [selectedFrom, setSelectedFrom] = React.useState(undefined);
    const [selectedTo, setSelectedTo] = React.useState(undefined);

    useEffect(() => {
        api
          .get(`api/profile/languages`)
          .then(res => setLanguages(res.data));
    }, [setLanguages]);

    const onClick = () => {
        const from = getLanguageKey(selectedFrom);
        const to = getLanguageKey(selectedTo);

        api
          .post(`api/profile/create?native=${from}&target=${to}`)
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
                <div className={classes.container}>
                    {languages.map(language => 
                    <li key={getLanguageKey(language)}>
                        <Chip
                            label={language.name}
                            onClick={onSelect(undefined, language)}
                        />
                    </li>
                    )}
                </div>
                <div className={classes.container}>
                    { selectedFrom !== undefined && 
                        <Chip
                            label={`Native ${selectedFrom.name}`}
                            onDelete={onSelect(selectedFrom, undefined)} />
                    }
                    { selectedTo !== undefined && 
                        <Chip
                            label={`Target ${selectedTo.name}`}
                            onDelete={onSelect(selectedTo, undefined)} /> 
                    }
                </div>
                <div className={classes.container}>
                    { isReady(selectedFrom, selectedTo, props.profiles) && 
                        <Button variant="contained" onClick={onClick}>Create</Button>
                    }
                </div>
            </Paper>            
        </div>
    );
}

export default ProfileCreateDialog;