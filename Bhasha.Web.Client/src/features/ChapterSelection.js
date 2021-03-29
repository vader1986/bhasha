import React, { useEffect } from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import axios from 'axios';

function ChapterSelection(props) {
    const [chapters, setChapters] = React.useState([]);

    useEffect(() => {
        axios
          .get(`api/chapter/list?profileId=${props.profile.id}`)
          .then(res => setChapters(res.data));
    }, [setChapters, props]);

    return (
        <div>
            <List component="nav">
                { chapters.map(chapter => 
                <ListItem button>
                    <ListItemText>{chapter.name}</ListItemText>
                </ListItem>)
                }
            </List>
        </div>
    );
}

export default ChapterSelection;