import React, { Component } from 'react';

export default class UserPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
        }
    }

    render() {
        console.log( 'props in userPage', this.props.state)
        return (
            <div className="container bootstrap snippet">

                <div className="row ng-scope">
                    <div className="col-md-4">
                        <div className="panel panel-default">
                            <div className="panel-body text-center">
                                <div className="pv-lg mr-3 ml-3"><img className="center-block img-circle img-responsive img-thumbnail rounded-circle thumb96" src="images/Avatar.png" alt="Contact" /></div>
                                <h3 className="m0 text-bold">Audrey Hunt</h3>
                                <div className="row justify-content-center">
                                    <div className="col-11">
                                        <textarea class="form-control" id="exampleTextarea" placeholder="User description" row="4"></textarea>
                                    </div>
                                </div>
                                <div className="text-center"><a className="btn btn-primary custom-btn mb-4 waves-effect #3abd94" href="">Send message</a></div>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-8 panel panel-default">
                        <div>
                            <div className="panel-body">
                                <div className="pull-right">
                                </div>
                                <div className="h4 text-center mr-md-5 mt-5 mt-md-3">Account Information</div>
                                <div className="row pv-lg">
                                    <div className="col-lg-2"></div>
                                    <div className="col-lg-8">
                                        <form className="form-horizontal ng-pristine ng-valid">
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact1">Name</label>
                                                <div className="col-md-10">
                                                    <input id="inputContact1" type="text" placeholder="Name" defaultValue="" />
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact2">Email</label>
                                                <div className="col-md-10">
                                                    <input id="inputContact2" type="email" placeholder="Email Address" defaultValue="" />
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact3">Phone</label>
                                                <div className="col-md-10">
                                                    <input id="inputContact3" type="text" placeholder="Phone number" defaultValue="" />
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact6">Address</label>
                                                <div className="col-md-10">
                                                    <textarea className="materialize-textarea" id="inputContact6" placeholder="Address" defaultValue=""row="4"/>
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <div className="col-sm-offset-2 col-sm-10">
                                                    <div className="checkbox">
                                                        <label>
                                                            <input type="checkbox" /> Favorite contact?
                                                    </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <div className="col-sm-offset-2 col-sm-10">
                                                    <button className="btn btn-primary custom-btn" type="submit">Update</button>
                                                </div>
                                            </div>
                                        </form>
                                        <div className="text-right mb-3"><a className="text-muted" href="#">Delete this contact?</a></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
