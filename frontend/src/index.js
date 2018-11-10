import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter as Router } from 'react-router-dom';
import { Route } from 'react-router-dom';
import App from './App';
import Admin from './Admin/Admin';
import './index.css';

ReactDOM.render(
  <Router>
    <div>
      <Route path="/main" component={App}/>
      <Route path="/admin" component={Admin}/>
    </div>
  </Router>, 
  document.getElementById('root')
);
