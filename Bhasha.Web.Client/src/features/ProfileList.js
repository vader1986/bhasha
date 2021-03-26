import { IconButton, ListItem, ListItemSecondaryAction, ListItemText } from '@material-ui/core';
import axios from 'axios';
import React from 'react';
import DeleteIcon from '@material-ui/icons/Delete';

const ProfileList = (props) => {

    const displayName = (profile) => {
        return `${profile.from.name} - ${profile.to.name}`;
    };

    const onDeleteProfile = (profile) => () => {
        axios
          .delete(`api/profile/delete?profileId=${profile.id}`)
          .then(_ => props.onDelete(profile));
    };

    const onSelect = (profile) => () => {
        props.onSelect(profile);
    };

    return (
        <div>
            {props.profiles.map(profile => 
                <ListItem button key={profile.id} onClick={onSelect(profile)}>
                    <ListItemText>{displayName(profile)}</ListItemText>
                    <ListItemSecondaryAction>
                    <IconButton
                        edge="end" 
                        aria-label="delete" 
                        onClick={onDeleteProfile(profile)}>
                      <DeleteIcon />
                    </IconButton>
                  </ListItemSecondaryAction>
                </ListItem>)
            }
        </div>
    );
};

export default ProfileList;