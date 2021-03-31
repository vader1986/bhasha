import { Card, CardContent, Chip, makeStyles, Paper, Typography } from '@material-ui/core';
import React from 'react';
import Page from './pages/Page';

const useStyles = makeStyles((theme) => ({
    root: {
      display: 'flex',
      justifyContent: 'center',
      flexWrap: 'wrap',
      listStyle: 'none',
      padding: theme.spacing(0.5),
      margin: 0,
    },
    title: {
      fontSize: 14,
    },
    chip: {
      margin: theme.spacing(0.5),
    },
  }));

function Chapter(props) {
    const classes = useStyles();
    const [result, setResult] = React.useState(undefined)

    const onSetResult = data => {
      setResult(data);
    };

    return (
        <div>
            <Card>
                <CardContent>
                    <Typography className={classes.title} color="textSecondary" gutterBottom>
                        Please select the correct solution!
                    </Typography>
                    <Typography>
                        {props.chapter.pages[0].translation.native}
                    </Typography>
                    { result !== undefined &&
                    <Typography>
                      Selected translation: {result}
                    </Typography>
                    }
                </CardContent>
            </Card>
            <Page page={props.chapter.pages[0]} onSetResult={onSetResult} />
        </div>
    );
}

export default Chapter;