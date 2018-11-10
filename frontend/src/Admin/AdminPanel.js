import React from 'react';
import {PropTypes} from 'prop-types';
import '../App.css';

import Companies from './Companies';
//import Departures from './Departures';
import NavBar from './NavBar';

class AdminPanel extends React.Component {

    constructor(props) {
        super(props);
        this.state = { 
            componentToShow: 0
        };
    }

    static propTypes = {
        url: PropTypes.string,
        //initialFilters: PropTypes.JSON,
        pollInterval: PropTypes.number
    }

    handleBarClick = (componentClicked) => {
        this.setState({ componentToShow : componentClicked });
    }

    render() {
        return (
            <div className="App">
                <div className="App-header">
                    <h2>Admin Panel</h2>
                </div>
                <NavBar onBarClick={this.handleBarClick}/>
                <div className="App-data-container">
                    {
                        this.state.componentToShow === 0 ?
                            <Companies url="http://localhost/api/v1/companies" pollInterval={50000} initialFilters={[]}/>
                        :
                            <Companies url="http://localhost/api/v1/companies" pollInterval={50000}/>
                    }
                </div>
                <div>
                    <form onSubmit={this.handleSubmit}>
                        <br></br>
                        <label htmlFor="CompanyName">Company Name:</label><br></br>
                        <input
                            id="CompanyName"
                            type="text"
                            value={this.state.ArrivesAt}
                            onChange={this.handleArrivesAtChange}
                        /><br></br>
                        
                        <label htmlFor="DepartsFrom">Departs from:</label><br></br>
                        <input
                            id="DepartsFrom"
                            type="text"
                            value={this.state.ArrivesFrom}
                            onChange={this.handleArrivesFromChange}
                        /><br></br>

                        <label htmlFor="ArrivesAt">Arrives at:</label><br></br>
                        <input
                            id="ArrivesAt"
                            type="text"
                            value={this.state.ArrivesFrom}
                            onChange={this.handleArrivesFromChange}
                        /><br></br>
                        <input className="button" type="submit" value="Submit" />
                    </form>
                </div>
            </div>
        );
    }
}

export default AdminPanel;