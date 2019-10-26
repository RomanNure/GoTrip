import React, { Component } from 'react';
import EmployeeList from '../components/EmployeeList.js';

export default class CompanyPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tab: "Information"
        }
    }
    _onChangeTab = (tab) => (e) => {
        e.preventDefault()
        console.log(' - tab => ', tab)
        this.setState({ tab })
    }

    render() {
        return (
            <div>
                <div className="container bootstrap snippet" >
                    <div className="row ng-scope">
                        <div className="col-md-4" >
                            <div className="panel panel-default" style={{ height: 600, backgroundColor: "#fff", borderRadius: 20 }}>
                                <div className="panel-body text-center">
                                    <div className="pv-lg mr-3 ml-3">
                                        <>
                                            <label htmlFor="Photo">
                                                <img className="center-block img-circle img-responsive img-thumbnail rounded-circle thumb96" src="images/Avatar.png" alt="Contact" />
                                            </label>
                                            <input type="file" ref='photo' id='Photo' style={{ display: "none" }} />

                                        </>
                                    </div>
                                    <h3 className="m0 text-bold"></h3>
                                    <div className="row justify-content-center">
                                        <div className="col-11">
                                            <textarea className="form-control" id="exampleTextarea" placeholder="Company description" row="4"></textarea>
                                        </div>
                                    </div>
                                    <div className="text-center" style={{ visibility: "hidden" }}><a className="btn btn-primary custom-btn mb-4 waves-effect #3abd94" href="">Send message</a></div>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-8 panel panel-default" style={{ backgroundColor: "#fff", borderRadius: 20 }}>
                            <div>
                                <div className="panel-body">
                                    <div className="pull-right">
                                    </div>
                                    <div className="h4 text-center mr-md-12 mt-12 mt-md-3" style={{ borderRadius: 20 }}>
                                        <a className={this.state.tab == 'Information' ? "btn-large waves-effect waves-light #81c784 green lighten-1 " : 'btn-large waves-effect waves-light #81c784 green lighten-2'} style={{ width: "35%" }} onClick={this._onChangeTab('Information')}>Information</a>
                                        <a className={this.state.tab == 'Tours' ? "btn-large waves-effect waves-light #81c784 green lighten-1 " : 'btn-large waves-effect waves-light #81c784 green lighten-2'} style={{ width: "35%" }} onClick={this._onChangeTab('Tours')}>Tours</a>
                                        <a className={this.state.tab == 'Employees' ? "btn-large waves-effect waves-light #81c784 green lighten-1 " : 'btn-large waves-effect waves-light #81c784 green lighten-2'} style={{ width: "30%" }} onClick={this._onChangeTab('Employees')}>Employees</a>
                                    </div>
                                    {this.state.tab == "Information" && <div className="row pv-lg">
                                        <div className="col-lg-2"></div>
                                        <div className="col-lg-8">
                                            <form className="form-horizontal ng-pristine ng-valid">
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact1">Name</label>
                                                    <div className="col-md-10">
                                                        <input ref="fullname" id="inputContact1" type="text" placeholder="Name" defaultValue="" value="test" />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact2">Email</label>
                                                    <div className="col-md-10">
                                                        <input ref="email" id="inputContact2" type="email" placeholder="Email Address" value="test" />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact3">Phone</label>
                                                    <div className="col-md-10">
                                                        <input ref='phone' id="inputContact3" type="text" placeholder="Phone number" defaultValue="" value="test" />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact6">Address</label>
                                                    <div className="col-md-10">
                                                        <textarea className="materialize-textarea" id="inputContact6" placeholder="Address" defaultValue="" row="4" />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <div className="col-sm-offset-2 col-sm-10">
                                                        <a className="btn waves-effect waves-light #81c784 green lighten-2">Update</a>
                                                    </div>
                                                </div>
                                            </form>
                                        </div>
                                    </div>
                                    }
                                    {this.state.tab == "Employees" && 
                                    <EmployeeList/>
                                }
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
