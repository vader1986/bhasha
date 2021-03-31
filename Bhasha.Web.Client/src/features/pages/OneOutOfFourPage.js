import { Chip, makeStyles, Paper } from '@material-ui/core';
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
  }));

function OneOutOfFourPage(props) {
    const classes = useStyles();
    const setResult = option => () => {
        props.onSetResult(option);
    };

    return (
        <Paper component="ul" className={classes.root}>
        { props.page.arguments.options.map(option => 
            <li key={option}>
                <Chip
                    label={option}
                    onClick={setResult(option)}
                    className={classes.chip}
                />
            </li>)
        }
        </Paper>
    );
}

export default OneOutOfFourPage;