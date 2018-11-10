import React from 'react';
import Folders from './Folders';
import Departures from './Departures';
import NavBar from './NavBar';

class Main extends React.Component {

    constructor(props) {
        super(props);
        this.state = { componentToShow: 0 }; // 0 - Folders, 1 - Departures, TODO should go in Enum
    }

    handleBarClick = (componentClicked) => {
        this.setState({ componentToShow : componentClicked });
    }

    render() {
        return (
            <div className="App-main">
                <NavBar onBarClick={this.handleBarClick}/>
                <div className="App-data-container">
                    {
                        this.state.componentToShow === 0 ?
                            <Folders url="http://localhost/api/v1/routes" pollInterval={50000} initialFilters={[{Property: 'Type', Value: 0, Operator: 'eq'}]}/>
                        :
                            <Departures url="http://localhost/api/v1/routes" pollInterval={50000} initialFilters={[{Property: 'Type', Value: 1, Operator: 'eq'}]}/>
                    }
                </div>
            </div>
        );
    }
}

export default Main;