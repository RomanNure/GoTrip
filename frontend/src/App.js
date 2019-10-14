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
import UserPage from './pages/UserPage.js';
import NotFound from './components/NotFound.js';

export default class App extends Component {
  constructor(props) {
    super(props);

    this.state = {

    }
  }
  /* eslint-disable */
  render() {

    console.log('router', this.props)
    return (
      <div className="container-fluid" style={{ backgroundColor: "#eee" }}>
        <Header />
        <div className="container-fluid" style={{ marginTop: 75 }}>
          <Switch>
            <Route path="/" exact component={Home} />
            <Route path="/login" exact component={SignIn} />
            <Route path="/userpage" exact component={UserPage} />

            <Route path="/registration" exact component={SignUp} />
            <Route path="/urer/:id" />
            <Route component={NotFound} />
          </Switch>
        </div>

      </div>
    )
  }
}
//<Route path="/" component={NotFound} />
