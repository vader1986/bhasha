import React, { useEffect } from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { Collapse, Paper } from '@material-ui/core';
import Chip from '@material-ui/core/Chip';
import axios from 'axios';

function ProfileList(props) {
    const [open, setOpen] = React.useState(false);
    const [profiles, setProfiles] = React.useState([]);
    const [languages, setLanguages] = React.useState([]);
    const [selectedFrom, setSelectedFrom] = React.useState(undefined);
    const [selectedTo, setSelectedTo] = React.useState(undefined);

    const handleClick = () => {
        setOpen(!open);
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

    return (
        <div>
            <List component="nav">
                <ListItem button key="create" onClick={handleClick}>
                    <ListItemText>Create Profile ...</ListItemText>
                </ListItem>
                <Collapse in={open} timeout="auto" unmountOnExit>
                    <Paper>
                        { languages.map(x => 
                        <li key={x.region !== undefined ? x.id + "_" + x.region : x.id}>
                            <Chip
                                label={x.name}
                                onDelete={onSelectLanguage(x)}
                            />
                        </li>
                        )}
                    </Paper>
                </Collapse>
                {profiles.map(profile => 
                <ListItem button key={profile.id}>
                    <ListItemText style={{color: '#005FFF'}}>
                        {profile.from.name} - {profile.to.name}
                    </ListItemText>
                </ListItem>)}
            </List>
        </div>
    );
}

export default ProfileList;