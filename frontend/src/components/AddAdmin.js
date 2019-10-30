import React, { PureComponent } from 'react';
import { ToastContainer, toast } from 'react-toastify';

export default class AddAdmin extends PureComponent {
    constructor(props) {
        super(props);
        this.state = {}
    }


    render() {
        return (
            <>
                <ToastContainer />
                <div className="container" style={{ height: 615 }}>
                    <div className="panel panel-default col-md-7 ml-auto mr-auto pb-2 pt-2 pl-4 pr-4" style={{ backgroundColor: "#fff", borderRadius: 15, top: 25 }}>
                        <div className="row text-center">
                            <div className="col-12 text-center">
                                <div className="h3">Add company administrator</div>
                            </div>
                        </div>
                        <div className="row justify-content-center">
                            <div className="col-12">
                                <form action="">

                                    <div className="form-group">
                                        <label htmlFor="user-email">User email</label>
                                        <input ref="email" type="email" className="form-control" id="company-email" placeholder="Enter email" />
                                    </div>

                                    <a className="btn waves-effect waves-light #81c784 green lighten-2">Add Admin</a>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </>
        )
    }
}

/*
<div className="form-group">
    <label htmlFor="company-name">Company name</label>
    <input ref="name" type="text" className="form-control" id="company-name" placeholder="Enter company name" />
</div>*/
