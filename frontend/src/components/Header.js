import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import cookie from 'react-cookies'

export default class Header extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: cookie.load("user") ? cookie.load("user") : false
        }
    }
    

    render() {
        let { login, id } = this.state.user
        if (!login) login = false

        console.log('header state=> ', this.state)
        return (
            <div>
                <nav>
                    <div className="nav-wrapper #81c784 green lighten-2">
                        <a href="/" className="brand-logo"><img src="./gotrip.svg" height="125" width="125" /></a>
                        <ul className="right hide-on-med-and-down">
                            <li><a href="sass.html"><i className="material-icons">search</i></a></li>
                            {!login && <li className="active"><Link to="/registration">Sign Up</Link></li>}
                            {!login && <li><Link to="/login">Sign In</Link></li>}
                            {login && <li id="username">
                                <a className="nav-link pr-2 pl-2 p-0" href={"/user:" + id}>{login}</a>
                            </li>
                            }
                            {login && <li className="nav-item avatar">
                                <a className="nav-link p-0" href={"/user:" + id}>
                                    <img src="images/Avatar.png" className="rounded-circle z-depth-0"
                                        id="header-avatar" alt="avatar image" height="35"></img>
                                </a>
                            </li>
                            }
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