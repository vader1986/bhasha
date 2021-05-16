import React, { useEffect } from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { api } from '../utils';
import { Done } from '@material-ui/icons';

function ChapterSelection(props) {
    const [envelopes, setChapterEnvelopes] = React.useState([]);
    const [loading, setLoading] = React.useState(true);

    const onSelect = (chapter) => () => {
        props.onSelect(chapter);
    }

    useEffect(() => {
        api
          .get(`api/chapter/list?profileId=${props.profile.id}`)
          .then(res => {
              setChapterEnvelopes(res.data);
              setLoading(false);
            });
    }, [setChapterEnvelopes, props]);

    if (loading)
    {
        return <div>... loading ...</div>
    }

    const createItem = envelope => {
        const completed = envelope.stats.completed || props.completedIds.includes(envelope.chapter.id);
        const chapter = envelope.chapter;

        return (
            <ListItem button onClick={onSelect(chapter)}>
                <ListItemText>{chapter.name.native.toUpperCase()}</ListItemText>
                {completed && <Done color="primary" />}
            </ListItem>);
    };

    return (
        <div>
            <List component="nav">
                { envelopes.map(createItem) }
            </List>
        </div>
    );
}

export default ChapterSelection;