import React, { Component } from 'react';
import { Link } from 'react-router-dom';
export default class Header extends Component {
    constructor(props) {
        super(props);
        this.state = {
        }
    }

    render() {
        return (
            <div>
                <nav>
                    <div className="nav-wrapper #81c784 green lighten-2">
                        <a href="/" className="brand-logo"><img src="./gotrip.svg" height="125" width="125" /></a>
                        <ul className="right hide-on-med-and-down">
                            <li><a href="sass.html"><i className="material-icons">search</i></a></li>
                            <li className="active"><Link to="/registration">Sign Up</Link></li>
                            <li><Link to="/login">Sign In</Link></li>
                            <li id="username">
                                <a className="nav-link pr-2 pl-2 p-0">User Name</a>
                            </li>
                            <li className="nav-item avatar">
                                <a className="nav-link p-0" href="#">
                                    <img src="images/Avatar.png" className="rounded-circle z-depth-0"
                                       id="header-avatar" alt="avatar image" height="35"></img>
                                </a>
                            </li>
                        </ul>
                   </div>
                </nav>
            </div>
        )
    }
}
/*
        <nav className="navbar navbar-expand-lg navbar-light bg-light">

                    <Link className="navbar-brand" to="/"><CardMedia image="gotrips.svg"/></Link>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                        <ul className="navbar-nav mr-auto">
                            <li className="nav-item active">
                                <a className="nav-link" href="#">Home <span className="sr-only">(current)</span></a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" href="#">Link</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" href="#">Disabled</a>
                            </li>
                        </ul>
                        <Link className="nav-item my-2 my-lg-0 pl-md-0" to="/registration">Create Account</Link>
                        <Link className="nav-item my-2 my-lg-0 pl-md-0" to="/login">Login</Link>
                    </div>
                </nav>
                */