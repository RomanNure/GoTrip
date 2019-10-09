import React, { Component } from 'react';
import Modal from '@material-ui/core/Modal';
import ReginstForm from './registerForm.js';
import SignIn from './SignIn.js';
import SignUp from './SignUp.js';

export default class Header extends Component {
    constructor(props) {
        super(props);
        this.state = {
            openLogin: false,
            openRegistr: false
        }
    }

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
    render() {
        return (
            <div>
                {this.state.openLogin &&
                    <Modal
                        aria-labelledby="modal-title"
                        aria-describedby="modal-description"
                        open={this.state.openLogin}
                        onBackdropClick={() => this.setState({ openLogin: false })}
                    >
                        <SignIn />
                    </Modal>
                }
                {this.state.openRegistr &&
                    <Modal
                        aria-labelledby="modal-title"
                        aria-describedby="modal-description"
                        open={this.state.openRegistr}
                        onBackdropClick={() => this.setState({ openRegistr: false })}
                    >
                        <SignUp />
                    </Modal>
                }
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
                        <a className="nav-item nav-link my-2 my-lg-0 pl-md-0" onClick={this._registration}>Create Account</a>
                        <a className="nav-item nav-link my-2 my-lg-0 pl-md-0" onClick={this._login}>Login</a>
                    </div>
                </nav>
            </div>

        )
    }
}