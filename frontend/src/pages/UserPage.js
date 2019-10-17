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
                                <div className="pv-lg mr-3 ml-3"><img className="center-block img-circle img-responsive img-thumbnail rounded-circle thumb96 mt-3" src="images/Avatar.png" alt="Contact" /></div>
                                <h3 className="m0 text-bold">Audrey Hunt</h3>
                                <div className="mv-lg">
                                    <input ref="descr" value="description"/>
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
                                                    <input id="inputContact2" type="email" placeholder="Name" defaultValue="" />
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact3">Phone</label>
                                                <div className="col-md-10">
                                                    <input id="inputContact3" type="text" defaultValue="" />
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact5">Website</label>
                                                <div className="col-md-10">
                                                    <input id="inputContact5" type="text" defaultValue="" />
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact6">Address</label>
                                                <div className="col-md-10">
                                                    <textarea className="materialize-textarea" id="inputContact6" defaultValue="lorem ipsum 69"row="4"/>
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact7">Social</label>
                                                <div className="col-md-10">
                                                    <input id="inputContact7" type="text" defaultValue="" />
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                <label className="col-sm-2 control-label" htmlFor="inputContact8">Company</label>
                                                <div className="col-md-10">
                                                    <input id="inputContact8" type="text" placeholder="No Company" />
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
