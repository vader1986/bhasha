import React, { useEffect } from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import axios from 'axios';

function ChapterSelection(props) {
    const [chapters, setChapters] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    const onSelect = (chapter) => () => {
        props.onSelect(chapter);
    }

    useEffect(() => {
        axios
          .get(`api/chapter/list?profileId=${props.profile.id}`)
          .then(res => {
              setChapters(res.data);
              setLoading(false);
            });
    }, [setChapters, props]);

    if (loading)
    {
        return <div>... loading ...</div>
    }

    return (
        <div>
            <List component="nav">
                { chapters.map(chapter => 
                <ListItem button onClick={onSelect(chapter)}>
                    <ListItemText>{chapter.name.toUpperCase()}</ListItemText>
                </ListItem>)
                }
            </List>
        </div>
    );
}

export default ChapterSelection;