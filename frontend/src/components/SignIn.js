import React, { Component } from 'react';
import axios from 'axios';

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
        axios.post("https://go-trip.herokuapp.com/authorize/", { login: login.value, password: pass.value })
            .then(data => console.log('data => ', data))
            .catch(e => console.log('error => ', e))
    }

    render() {
        console.log(' - SignIn')
        return (
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
                        <a className="btn waves-effect waves-light #e1f5fe light-blue lighten-5"
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
        );
    }
}