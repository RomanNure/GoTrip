import React, { PureComponent } from "react";


export default class RegisterForm extends PureComponent {
    constructor(props) {
        super(props);
        this.state = {}
    }

    render() {
        return (
            <div class="modal fade" id="registerModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Create Account</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <form action="">
                                <div class="form-group">
                                    <label for="inputEmailReg">Email address</label>
                                    <input type="email" class="form-control" id="inputEmailReg" aria-describedby="emailHelp" placeholder="Enter email" />
                                </div>
                                <div class="form-group">
                                    <label for="inputLoginReg">Login</label>
                                    <input type="text" class="form-control" id="inputLoginReg" aria-describedby="emailHelp" placeholder="Enter login" />
                                </div>
                                <div class="form-group">
                                    <label for="inputPasswordReg">Password</label>
                                    <input type="password" class="form-control" id="inputPasswordReg" placeholder="Password" />
                                </div>
                                <div class="form-group">
                                    <label for="inputPasswordReEnterReg">Re-enter password</label>
                                    <input type="password" class="form-control" id="inputPasswordReEnterReg" placeholder="Password" />
                                </div>
                                <div class="form-check">
                                    <input type="checkbox" class="form-check-input" id="checkReg" />
                                    <label class="form-check-label" for="checkReg">Check me out</label>
                                </div>
                                <button type="submit" class="btn btn-success mt-3 mb-3">Register</button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

}