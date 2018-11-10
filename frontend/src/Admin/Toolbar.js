import React from 'react';
import {PropTypes} from 'prop-types';

class Toolbar extends React.Component {

    constructor(props) {
        super(props);
        this.state = { activeComponent: 0 };
    }

    static propTypes = {
        onBarClick: PropTypes.func
    }

    handleAddClick = (e) => {

        e.preventDefault();
        this.props.onBarClick(0);
        this.setState({activeComponent : 0});
    }

    handleDeleteClick = (e) => {

        e.preventDefault();
        this.props.onBarClick(0);
        this.setState({activeComponent : 0});
    }

    handleUpdateClick = (e) => {

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
            <div className="toolbar">
                <button className="button" onClick={this.handleAddClick}>Add</button>
                <button className="button" onClick={this.handleDeleteClick}>Delete</button>
                <button className="button" onClick={this.handleUpdateClick}>Update</button>
            </div>
        );
    }
}

export default Toolbar;