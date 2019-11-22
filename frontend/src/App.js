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
import BecomeGuide from "./components/BecomeGuide";

import NotFound from './components/NotFound.js';
import cookie from 'react-cookies'
import Footer from './components/Footer.js';
import ToursList from "./components/ToursList";
import AddAdmin from "./components/AddAdmin";
import TourPage from "./components/TourPage";
import GuideList from './components/GuideList.js';

const GlobalContext = React.createContext();
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
      <GlobalContext.Provider value={{
        user: this.state.user
      }}>

        <div style={{ display: "flex", width: "100%", height: "100%", flexDirection: "column", justifyContent: "space-between", backgroundColor: "#eee" }}>
          <Header />
          <div className="container-fluid" style={{ display: "flex", minHeight: 750 }}>
            <Switch>
              <Route path="/" exact component={Home} />
              <Route path="/login" exact component={SignIn} />

              <Route path="/create-company" exact component={CreateCompany} />
              <Route path="/add-admin" exact component={AddAdmin} />
              <Route path="/tour-page" exact component={TourPage} />
              <Route path="/employee-list" exact component={EmployeeList} />
              <Route path="/tours-list" exact component={ToursList} />

              <Route path="/become-guide" exact component={BecomeGuide} />
              <Route path="/guide-list" exact component={GuideList} />

              <Route path="/create-company" exact component={CreateCompany} />
              <Route path="/add-admin" exact component={AddAdmin} />
              <Route path="/tour-page" exact component={TourPage} />
              <Route path="/create-tour" exact component={TourPage} />

              <Route path="/employee-list" exact component={EmployeeList} />
              <Route path="/tours-list" exact component={ToursList} />
              <Route path="/become-guide" exact component={BecomeGuide} />

              <Route path="/user:id" component={(data) => <UserPage {...data} />} />
              <Route path="/company:id" component={(data) => <CompanyPage {...data} />} />

              <Route component={NotFound} />
            </Switch>
          </div>
          <Footer />
        </div>
      </GlobalContext.Provider>
    )
  }
}
//<Route path="/" component={NotFound} />
