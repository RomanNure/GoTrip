import React, { Component } from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from "react-router-dom";
import SignIn from './components/SignIn.js';
import Home from './pages/Home.js';
import SignUp from './components/SignUp.js';
import Header from './components/Header.js';
import NotFound from './components/NotFound.js';

export default class App extends Component {
  constructor(props) {
    super(props);

    this.state = {

    }
  }

  render() {
    console.log('router', this.props)
    return (
      <>
        <Header />

          {/* A <Switch> looks through its children <Route>s and
            renders the first one that matches the current URL. */}
            <Route path="/" component={Home} />
            <Route path="/login" component={SignIn} />
            <Route path="/registration" component={SignUp} />

      </>
    )
  }
}
//<Route path="/" component={NotFound} />