import React, { PureComponent } from 'react';
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

import CreateCompany from "./components/CreateCompany";
import CompanyPage from "./pages/CompanyPage";
import EmployeeList from "./components/EmployeeList";

import NotFound from './components/NotFound.js';
import cookie from 'react-cookies'

export default class App extends PureComponent {
  constructor(props) {
    super(props);

    this.state = {
      user: false
    }
  }

  shouldComponentUpdate(p, s) {
    let cookieUser = cookie.load('user')
    console.log('cooki user', cookieUser, s)
    if (cookieUser && cookieUser.login != s.user.login) {
        console.log('marched rerender')
      return true
    }
    return false
  }
  /* eslint-disable */
  render() {

    console.log('- router rendered', this.props)
    return (
      <div className="container-fluid" style={{ backgroundColor: "#eee" }}>
        <Header />
        <div className="container-fluid" style={{ marginTop: 75, height: 790 }}>
          <Switch>
            <Route path="/" exact component={Home} />
            <Route path="/login" exact component={SignIn} />

            <Route path="/registration" exact component={SignUp} />

            <Route path="/create-company" exact component={CreateCompany} />
            <Route path="/company-page" exact component={CompanyPage} />
            <Route path="/employee-list" exact component={EmployeeList} />

            <Route path="/user:id" component={(data) => <UserPage {...data} />} />
            <Route component={NotFound} />
          </Switch>
        </div>

      </div>
    )
  }
}
//<Route path="/" component={NotFound} />
