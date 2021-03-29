import { Card, CardContent, Chip, makeStyles, Paper, Typography } from '@material-ui/core';
import React from 'react';

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

    return (
        <div>
            <Card>
                <CardContent>
                    <Typography className={classes.title} color="textSecondary" gutterBottom>
                        {props.chapter.name}
                    </Typography>
                    <Typography>
                        {props.chapter.pages[0].translation.native}
                    </Typography>
                </CardContent>
            </Card>
            <Paper component="ul" className={classes.root}>
            { props.chapter.pages[0].pageType === 0 && 
              props.chapter.pages[0].arguments.options.map(option => 
                <li key={option}>
                    <Chip
                        label={option}
                        onClick={() => {alert(option)}}
                        className={classes.chip}
                    />
                </li>)
            }
            </Paper>
        </div>
    );
}

export default Chapter;