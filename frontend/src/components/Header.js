import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export default class Header extends Component {
    constructor(props) {
        super(props);
        this.state = {
        }
    }
    /*
        _login = (e) => {
            console.log('- login')
            e.preventDefault();
            this.setState({ openLogin: true })
        }
    
        _registration = (e) => {
            console.log('- refistration')
            e.preventDefault();
            this.setState({ openRegistr: true })
        }
    */
    render() {
        return (
            <div>
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <a className="navbar-brand" href="#">Go&Trip</a>
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
                        <Link className="nav-item nav-link my-2 my-lg-0 pl-md-0" to="/registration">Create Account</Link>
                        <Link className="nav-item nav-link my-2 my-lg-0 pl-md-0" to="/login">Login</Link>
                    </div>
                </nav>
            </div>

        )
    }
}