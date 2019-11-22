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
        let { login, id, avatarUrl } = this.state.user
        if (!login) login = false

        console.log('header state=> ', this.state)
        return (
            <nav >
                <div className="nav-wrapper #81c784 green lighten-2" >
                    <a href="/" className="brand-logo"><img src="./gotrip.svg" style={{ height: 125, width: 125, marginLeft:10 }} /></a>
                    <ul className="right hide-on-med-and-down">
                        <li><a href="sass.html"><i className="material-icons" style={{ marginRight: 15, alignItems: "center" }}>search</i></a></li>
                        {login ?
                            <li className="nav-item avatar">
                                <a className="nav-link p-0" href={"/user:" + id}>
                                    {login}
                                    <img src={avatarUrl ? avatarUrl : "images/Avatar.png"}
                                        alt="avatar image"
                                        style={{ height: 60, width: 60, borderRadius: 100, margin: 10, marginTop: 2, marginBottom: 5 }}
                                    />
                                </a>
                            </li>
                            :
                            <>
                                <li className="active"><Link to="/registration">Sign Up</Link></li>
                                <li><Link to="/login">Sign In</Link></li>
                            </>
                        }
                    </ul>
                </div>
            </nav>
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