import React from 'react';
import $ from 'jquery';
import {PropTypes} from 'prop-types';
import Login from './Login';
import AdminPanel from './AdminPanel';

class Admin extends React.Component {

    constructor(props) {
        super(props);
        this.state = { 
            isLoggedIn: false,
            token: null
        };
    }

    static propTypes = {
        url: PropTypes.string,
        //initialFilters: PropTypes.JSON,
        pollInterval: PropTypes.number
    }

    handleLoginSubmit = (username, password) => {
        $.post({
            url: 'http://localhost/token',
            data: 'username=' + username + '&password=' + password + '&grant_type=password',
            async: true,
            cache: false,
            success: function (data) {
                this.setState({ token: data['access_token'] });
                this.setState({isLoggedIn: true});
            }.bind(this),
            error: function (xhr, status, err) {
                console.error("token", status, err.toString());
                alert("Authentication failed!");
            }
        });
    }

    render() {
        return (
            <div>
                {
                    this.state.isLoggedIn === false ?
                        <Login onLoginSubmit={this.handleLoginSubmit}/>
                    :
                        <AdminPanel/>
                }
            </div>
        );
    }
}

export default Admin;