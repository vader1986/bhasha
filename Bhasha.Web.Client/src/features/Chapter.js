import { Card, CardContent, IconButton, makeStyles, Typography } from '@material-ui/core';
import React from 'react';
import Page from './pages/Page';
import { api } from '../utils';
import { AssignmentTurnedInOutlined, ContactSupportOutlined } from '@material-ui/icons';

const useStyles = makeStyles((theme) => ({
    appBar: {
      position: "fixed",
      bottom: 0,
      left: 0,
      width: "100%"
    },
    appBarContent: {
      display: "flex",
      justifyContent: "center",
      width: "100%"
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
    const [result, setResult] = React.useState(undefined);
    const [pageIndex] = React.useState(0);
    const currentPage = props.chapter.pages[pageIndex];

    const onSubmit = () => {
      const args = `profileId=${props.profile.id}&chapterId=${props.chapter.id}&pageIndex=${pageIndex}&solution=${result}`;
      api
        .post(`api/page/submit?${args}`)
        .then(response => alert(JSON.stringify(response.data)));
    };

    const onSetResult = data => {
      setResult(data);
    };

    const onTipClicked = () => {
      const args = `profileId=${props.profile.id}&chapterId=${props.chapter.id}`;
      api
        .post(`api/page/tip?${args}`, currentPage.tipIds)
        .then(response => alert(response.data));
    };

    return (
        <div>
            <Card>
                <CardContent>
                    <Typography className={classes.title} color="textSecondary" gutterBottom>
                        Please select the correct solution!
                    </Typography>
                    <Typography>
                        {currentPage.translation.native}
                    </Typography>
                    { result !== undefined &&
                    <Typography>
                      Selected translation: {result}
                    </Typography>
                    }
                </CardContent>
            </Card>
            <Page page={currentPage} onSetResult={onSetResult} />
            <div className={classes.appBar}>
              <div className={classes.appBarContent}>
                { currentPage.tipIds.length > 0 &&
                <IconButton onClick={onTipClicked}>
                  <ContactSupportOutlined />
                </IconButton>
                }
                { result !== undefined &&
                <IconButton onClick={onSubmit}>
                  <AssignmentTurnedInOutlined />
                </IconButton>
                }
              </div>
            </div>
        </div>
    );
}

export default Chapter;