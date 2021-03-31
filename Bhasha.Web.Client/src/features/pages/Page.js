import React from 'react';
import OneOutOfFourPage from './OneOutOfFourPage';

function Page(props) {
    switch (props.page.pageType)
    {
        case 0:
            return <OneOutOfFourPage {...props} />;

        default:
            return <OneOutOfFourPage {...props} />;
    }
}

export default Page;