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
import Footer from './components/Footer.js';
import ToursList from "./components/ToursList";

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
      <div style={{ backgroundColor: "#eee", position:"absolute", top:0, left:0, right:0, bottom:0}}>
        <Header />
        <div className="container-fluid" style={{ minHeight: 640 }}>
          <Switch>
            <Route path="/" exact component={Home} />
            <Route path="/login" exact component={SignIn} />

            <Route path="/registration" exact component={SignUp} />

            <Route path="/create-company" exact component={CreateCompany} />
            <Route path="/employee-list" exact component={EmployeeList} />
            <Route path="/tours-list" exact component={ToursList} />

            <Route path="/user:id" component={(data) => <UserPage {...data} />} />
            <Route path="/company:id" component={(data) => <CompanyPage {...data} />} />

            <Route component={NotFound} />
          </Switch>
        </div>
        <Footer />

      </div>
    )
  }
}
//<Route path="/" component={NotFound} />
