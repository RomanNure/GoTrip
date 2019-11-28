import React, { Component } from 'react';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import cookie from 'react-cookies'
import { register } from '../api.js';
const display = {
    display: 'block',
    top: 70,
    width: 500,
    borderRadius: 50
};
const hide = {
    display: 'none'
};


export default class SignUp extends Component {
    constructor(props) {
        super(props);
        this.state = {
            toggle: true,
            checked: false
        }
    }

    _onSubmit = () => {
        let { email, p1, p2, login } = this.refs
        console.log(' - onSubmit()')
        const EMAIL = /[\w\.-]+@\w{3,5}\.{1}\w{2,3}([\w]{2,3})?/ig
        const LOGIN = /(\w{8,20})/ig
        const PASSWORD = /\w{8,30}/ig
        if (!login.value) {
            toast.error(" No Login !", { position: toast.POSITION.TOP_RIGHT });
            return
        }
        if (!email.value) {
            toast.error(" No Email !", { position: toast.POSITION.TOP_RIGHT });
            return
        }
        if (!p1.value) {
            toast.error(" No Password !", { position: toast.POSITION.TOP_RIGHT });
            return
        }
        if (!p2.value) {
            toast.error("No confirmed password !", { position: toast.POSITION.TOP_RIGHT });
            return
        }
        if (!LOGIN.test(login.value)) {
            toast.error("Incorrect Login !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        if (!EMAIL.test(email.value)) {
            toast.error("Incorrect email !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        if (p1.value !== p2.value) {
            toast.error("Password mismatch !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        } else {
            if (!PASSWORD.test(p1)) {
                toast.error("Password must contain A-Z, a-z, 1-9 !", {
                    position: toast.POSITION.TOP_RIGHT
                });
            }
        }
        //console.log('values =>', login.value, email.value, p1.value)

        let user = {
            login: login.value,
            email: email.value,
            password: p1.value
        }
        register(user)
            .then(({ data }) => {
                toast.success("Registered", {
                    position: toast.POSITION.TOP_RIGHT
                });
                //console.log(`POST: user is added`, data);
                cookie.save('user', {...user }, { path: '/' })
                this.context.setUser(user)
                setTimeout(() => this.props.history.push({ pathname: '/user:' + data.id, state: data }), 2000)//, {props: data})
                // append to DOM
            })
            .catch(error => {

                if (error.response) {
                    console.log('data=>', error.response.data);
                    console.log("status=>", error.response.status);
                    console.log('headers =>', error.response.headers);
                    toast.error(error.response.data.message, {
                        position: toast.POSITION.TOP_RIGHT
                    });

                } else if (error.request) {
                    console.log('request err', error.request);
                } else {
                    console.log('Error', error.message);
                }
                console.log('config', error.config)
                console.log('Error', error);

            });

    }

    render() {
        return (
            <>
                <ToastContainer />
                <div className="modal" style={this.state.toggle ? display : hide}>
                    <div className="modal-content" style={{ paddingTop: 50, justifyContent: "center" }}>
                        <div className="row" style={{ justifyContent: "center" }}>
                            <i className="material-icons ">lock</i>
                            <h4>Sign Up</h4>
                        </div>
                        <div className="row" style={{ marginLeft: 5, marginRight: 5, height: 50 }}>
                            <div className="input-field col s6">
                                <input ref="login" placeholder="Login" id="login" type="text" className="validate" />
                            </div>
                        </div>
                        <div className="row" style={{ marginLeft: 5, marginRight: 5, height: 50 }}>
                            <div className="input-field col s6">
                                <input ref="email" placeholder="Email " id="email" type="email" className="validate" />
                            </div>
                        </div>
                        <div className="row" style={{ marginLeft: 5, marginRight: 5, height: 50 }}>
                            <div className="input-field col s6">
                                <input ref="p1" placeholder="Password" id="password1" type="password" className="validate" />
                            </div>
                        </div>
                        <div className="row" style={{ marginLeft: 5, marginRight: 5, height: 50 }}>
                            <div className="input-field col s6">
                                <input ref="p2" placeholder="Confirm password" id="password2" type="password" className="validate" />
                            </div>
                        </div>
                        <div className="row" style={{ marginTop: 5, marginLeft: 7 }}>
                            <label>
                                <input type="checkbox" className="filled-in" checked={this.state.checked} onChange={() => this.setState({ checked: !this.state.checked })} />
                                <span>I want receive message and updates via email</span>
                            </label>
                        </div>
                        <div className="row" style={{ justifyContent: "center" }}>
                            <a className="btn waves-effect waves-light #81c784 green lighten-2"
                                onClick={this._onSubmit} style={{ width: "90%", alignContent: "center" }}>Sign Up</a>
                        </div>
                        <div className="row">
                            <a href="/login"> Already have an account? Sign In</a>
                        </div>
                    </div>
                    <div className="modal-footer" style={{ justifyContent: "center" }}>
                        <div>Copyright @ Go&Trip 2019.</div>
                    </div>
                </div>
            </>
        );
    }
}
