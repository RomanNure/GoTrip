import React, { Component } from 'react';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import cookie from 'react-cookies'
import GlobalContext from '../GlobalContext.js';

const display = {
    display: 'block',
    top: 70,
    width: 450,
    height: 550,
    borderRadius: 50
};
const hide = {
    display: 'none'
};
export default class SignIn extends Component {
    static contextType = GlobalContext
    constructor(props) {
        super(props);
        this.state = {
            toggle: true,
            checked: false
        }
    }
    _onSubmit = () => {
        let { login, pass } = this.refs;
        console.log('post', login.value, pass.value)
        if (!login.value) {
            toast.error("Please type your login !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        if (!pass.value) {
            toast.error("Please type your password !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        axios({
            method: "post",
            url: 'https://go-trip.herokuapp.com/authorize',
            //url:'http://93.76.235.211:5000/authorize',    
            headers: {
                //"Content-Type": "text/plain",
                'Content-Type': 'application/json',//Content-Type': 'appication/json',
            },
            data: {
                login: login.value,
                password: pass.value
            },
        })
            .then(({ data }) => {
                toast.success("Weclome back!", {
                    position: toast.POSITION.TOP_RIGHT
                })
                console.log('data', data)
                cookie.save('user', { ...data }, { path: '/' })
                this.context.setUser(data)
                setTimeout(() => this.props.history.push({ pathname: '/user:' + cookie.load('user').id }), 4000)
                //setTimeout(() =>  window.location.update, 4000)//, {props: data})
                //window.location.reload();

            })
            .catch(err => {
                console.log(' - error in signIn', err)
                toast.error("User not found !", {
                    position: toast.POSITION.TOP_RIGHT
                });
            })
    }

    render() {
        console.log(' - SignIn')
        return (
            <>
                <ToastContainer />

                <div className="modal" style={this.state.toggle ? display : hide}>
                    <div className="modal-content" style={{ paddingTop: 50, justifyContent: "center" }}>
                        <div className="row" style={{ justifyContent: "center" }}>
                            <i className="material-icons ">lock</i>
                            <h4>Sign In</h4>
                        </div>
                        <div className="row" style={{ margin: 5 }}>
                            <div className="input-field col s6">
                                <input ref='login' placeholder="Login" id="login" type="text" className="validate" />
                            </div>
                        </div>
                        <div className="row" style={{ margin: 5 }}>
                            <div className="input-field col s6">
                                <input ref='pass' placeholder="Password" id="password" type="password" className="validate" />
                            </div>
                        </div>
                        <div className="row" style={{ marginLeft: 7 }}>
                            <label>
                                <input type="checkbox" className="filled-in" checked={this.state.checked} onChange={() => this.setState({ checked: !this.state.checked })} />
                                <span>Rememder me</span>
                            </label>
                        </div>
                        <div className="row" style={{ justifyContent: "center" }}>
                            <a className="btn waves-effect waves-light #81c784 green lighten-2"
                                onClick={this._onSubmit} style={{ width: "90%", alignContent: "center" }}>Sign In</a>
                        </div>
                        <div className="row">
                            <a className="col s6" href="/registration"> Forgot password</a>
                            <a href="/registration"> Don't have an account? Sign Up</a>
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