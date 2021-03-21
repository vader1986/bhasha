import React from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { Collapse, Paper } from '@material-ui/core';

function ProfileList(props) {
    const [open, setOpen] = React.useState(true);
    const handleClick = () => {
        setOpen(!open);
    };

    return (
        <div>
            <List component="nav">
                <ListItem button key="create" onClick={handleClick}>
                    <ListItemText>Create Profile ...</ListItemText>
                </ListItem>
                <Collapse in={open} timeout="auto" unmountOnExit>
                    <Paper>
                        asdasds
                        asd
                        a
                        sdasd
                    </Paper>
                </Collapse>
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