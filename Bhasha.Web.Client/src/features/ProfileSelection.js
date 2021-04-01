import React, { useEffect } from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { Collapse } from '@material-ui/core';
import ProfileCreateDialog from './ProfileCreateDialog';
import ProfileList from './ProfileList';
import axios from 'axios';

function ProfileSelection(props) {
    const [open, setOpen] = React.useState(false);
    const [profiles, setProfiles] = React.useState([]);

    const onExpandCreate = () => {
        setOpen(!open);
    };

    const onCreate = (profile) => {
        setOpen(!open);
        setProfiles(prev => prev.concat([profile]));
    };

    const onSelect = (profile) => {
        props.onSelect(profile);
    };

    const onDelete = (profile) => {
        setProfiles(prev => prev.filter(x => x !== profile));
    };

    useEffect(() => {
        axios
          .get(`api/profile/list`)
          .then(res => setProfiles(res.data));
    }, [setProfiles]);

    return (
        <div>
            <List component="nav">
                <ListItem button key="create" onClick={onExpandCreate}>
                    <ListItemText>Create Profile ...</ListItemText>
                </ListItem>
                <Collapse in={open} timeout="auto" unmountOnExit>
                    <ProfileCreateDialog profiles={profiles} onCreate={onCreate} />
                </Collapse>
                <ProfileList profiles={profiles} onDelete={onDelete} onSelect={onSelect} />
            </List>
        </div>
    );
}

export default ProfileSelection;