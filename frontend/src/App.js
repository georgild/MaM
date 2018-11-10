import React, { Component } from 'react';
import './App.css';

import Main from './Main';

class App extends Component {
  render() {
    return (
      <div className="App">
        <div className="App-header">
          <h2>Media Asset Management</h2>
        </div>
        {/*<p className="App-intro">
          To get started, edit <code>src/App.js</code> and save to reload.
        </p>*/}
        <Main/>
      </div>
    );
  }
}

export default App;
