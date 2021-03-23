import React, { useEffect } from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { Button, Collapse, IconButton, ListItemSecondaryAction, Paper } from '@material-ui/core';
import Chip from '@material-ui/core/Chip';
import axios from 'axios';
import DeleteIcon from '@material-ui/icons/Delete';

const getLangKey = (lang) => {
    return lang.region !== undefined && lang.region !== null 
        ? lang.id + "_" + lang.region : lang.id;
}

function ProfileList(props) {
    const [open, setOpen] = React.useState(false);
    const [profiles, setProfiles] = React.useState([]);
    const [languages, setLanguages] = React.useState([]);
    const [selectedFrom, setSelectedFrom] = React.useState(undefined);
    const [selectedTo, setSelectedTo] = React.useState(undefined);

    const handleClick = () => {
        setOpen(!open);
    };

    const onCreate = () => {
        setOpen(!open);
        axios
          .post(`api/profile/create?from=${getLangKey(selectedFrom)}&to=${getLangKey(selectedTo)}`)
          .then(res => {
              profiles.push(res.data);
              setProfiles(profiles);
          });
        axios
          .get(`api/profile/languages`)
          .then(res => setLanguages(res.data));

        setSelectedFrom(undefined);
        setSelectedTo(undefined);
    };

    const onDeleteProfile = (profile) => () => {
        axios
          .delete(`api/profile/delete?profileId=${profile.id}`)
          .then(_ => setProfiles(profiles.filter(x => x.id !== profile.id)));
    };

    const onSelectLanguage = (lang) => () => {
        if (selectedFrom === undefined) {
            setSelectedFrom(lang);
        }
        else
        if (selectedTo === undefined) {
            setSelectedTo(lang);
        }
        setLanguages(languages.filter(x => x !== lang));
    };

    const onUnselectLanguage = (lang) => () => {
        if (selectedFrom === lang) {
            setSelectedFrom(undefined);
        }
        else
        if (selectedTo === lang) {
            setSelectedTo(undefined);
        }
        languages.push(lang);
        setLanguages(languages);
    };

    useEffect(() => {
        axios
          .get(`api/profile/languages`)
          .then(res => setLanguages(res.data));
    }, [setLanguages]);

    useEffect(() => {
        axios
          .get(`api/profile/list`)
          .then(res => setProfiles(res.data));
    }, [setProfiles]);

    const validProfile = 
        selectedFrom !== undefined &&
        selectedTo !== undefined &&
        !profiles.some(x => 
            getLangKey(x.from) === getLangKey(selectedFrom) && 
            getLangKey(x.to) === getLangKey(selectedTo));

    return (
        <div>
            <List component="nav">
                <ListItem button key="create" onClick={handleClick}>
                    <ListItemText>Create Profile ...</ListItemText>
                </ListItem>
                <Collapse in={open} timeout="auto" unmountOnExit>
                    <Paper>
                        { languages.map(x => 
                        <li key={getLangKey(x)}>
                            <Chip
                                label={x.name}
                                onClick={onSelectLanguage(x)}
                            />
                        </li>
                        )}
                        { selectedFrom !== undefined && 
                            <div>
                                FROM
                                <Chip
                                    label={selectedFrom.name}
                                    onDelete={onUnselectLanguage(selectedFrom)} />
                            </div> }
                        { selectedTo !== undefined && 
                            <div>TO 
                                <Chip
                                    label={selectedTo.name}
                                    onDelete={onUnselectLanguage(selectedTo)} />
                            </div> }
                        { validProfile && 
                            <Button 
                                variant="contained" 
                                onClick={onCreate}>Create</Button>}
                    </Paper>
                </Collapse>
                {profiles.map(profile => 
                <ListItem button key={profile.id}>
                    <ListItemText style={{color: '#005FFF'}}>
                        {profile.from.name} - {profile.to.name}
                    </ListItemText>
                    <ListItemSecondaryAction>
                    <IconButton edge="end" aria-label="delete" onClick={onDeleteProfile(profile)}>
                      <DeleteIcon />
                    </IconButton>
                  </ListItemSecondaryAction>
                </ListItem>)}
            </List>
        </div>
    );
}

export default ProfileList;