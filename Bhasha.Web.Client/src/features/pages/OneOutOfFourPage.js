import { Chip, makeStyles, Paper } from '@material-ui/core';
import React from 'react';

const useStyles = makeStyles((theme) => ({
    root: {
      display: 'flex',
      justifyContent: 'center',
      flexWrap: 'wrap',
      listStyle: 'none',
      padding: theme.spacing(0.5),
      margin: 0
    },
  }));

function OneOutOfFourPage(props) {
    const classes = useStyles();
    const setSolution = option => () => {
        props.onSetSolution(option);
    };

    return (
        <Paper component="ul" className={classes.root}>
        { props.page.arguments.options.map(option => 
            <li key={option.value}>
                <Chip
                    label={option.displayName}
                    onClick={setSolution(option.value)}
                    className={classes.chip}
                />
            </li>)
        }
        </Paper>
    );
}

export default OneOutOfFourPage;