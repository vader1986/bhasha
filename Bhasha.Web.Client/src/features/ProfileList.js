import React from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';

function ProfileList(props) {
    return (
        <div>
            <List component="nav">
                <ListItem button key="create">
                    <ListItemText>Create Profile ...</ListItemText>
                </ListItem>
                {props.profiles.map(profile => 
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