import React from 'react';
import {PropTypes} from 'prop-types';

class NavBar extends React.Component {

    constructor(props) {
        super(props);
        this.state = { activeComponent: 0 };
    }

    static propTypes = {
        onBarClick: PropTypes.func
    }

    handleCompaniesClick = (e) => {

        e.preventDefault();
        this.props.onBarClick(0);
        this.setState({activeComponent : 0});
    }

    /*handleDeparturesClick = (e) => {

        e.preventDefault();
        this.props.onBarClick(1);
        this.setState({activeComponent : 1});
    }*/

    render() {
        return (
            <ul className="nav-bar">
                <li><a href="#" className={this.state.activeComponent === 0 ? 'active' : ''} onClick={this.handleCompaniesClick}>Companies</a></li>
            </ul>
        );
    }
}

export default NavBar;