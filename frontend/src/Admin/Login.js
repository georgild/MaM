import React from 'react';
import {PropTypes} from 'prop-types';

class Login extends React.Component {

    constructor(props) {
        super(props);
        this.state = { 
            Username: 'admin@admin.com',
            Password: '1234'
        };
    }

    static propTypes = {
        onLoginSubmit: PropTypes.func
    }

    handleSubmit = (e) => {
        e.preventDefault();

        var Username = this.state.Username.trim();
        var Password = this.state.Password.trim();

        this.props.onLoginSubmit(Username, Password);
    }

    handleUsernameChange = (e) => {
        this.setState({ Username: e.target.value });
    }

    handlePasswordChange = (e) => {
        this.setState({ Password: e.target.value });
    }

    render() {
        return (
            <div id="login-panel">
                <form onSubmit={this.handleSubmit}>
                    <label htmlFor="Username">Username:</label>
                    <input
                        id="Username"
                        type="text"
                        value={this.state.Username}
                        onChange={this.handleUsernameChange}
                    />
                    <label htmlFor="Password">Password:</label>
                    <input
                        id="Password"
                        type="password"
                        value={this.state.Password}
                        onChange={this.handlePasswordChange}
                    />
                    <input className="button" type="submit" value="Login" />
                </form>
            </div>
        );
    }
}

export default Login;