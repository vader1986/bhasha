import React, { useEffect } from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { Collapse, IconButton, ListItemSecondaryAction } from '@material-ui/core';
import axios from 'axios';
import DeleteIcon from '@material-ui/icons/Delete';
import ProfileCreateDialog from './ProfileCreateDialog';

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

    const onDeleteProfile = (profile) => () => {
        axios
          .delete(`api/profile/delete?profileId=${profile.id}`)
          .then(_ => setProfiles(profiles.filter(x => x.id !== profile.id)));
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
                {profiles.map(profile => 
                <ListItem button key={profile.id}>
                    <ListItemText style={{color: '#005FFF'}}>
                        {profile.from.name} - {profile.to.name}
                    </ListItemText>
                    <ListItemSecondaryAction>
                    <IconButton
                        edge="end" 
                        aria-label="delete" 
                        onClick={onDeleteProfile(profile)}>
                      <DeleteIcon />
                    </IconButton>
                  </ListItemSecondaryAction>
                </ListItem>)}
            </List>
        </div>
    );
}

export default ProfileSelection;