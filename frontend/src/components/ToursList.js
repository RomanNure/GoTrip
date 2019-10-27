import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export default class ToresList extends Component {
    constructor(props) {
        super(props);
        this.state = {}
    }


    render() {
        return (
            <div>
                <div className="container">
                    <div className="row">
                        <div className="col-md-8">
                            <div className="card rounded mb-4">
                                <img className="card-img-top" src="./images/bestplaceholder.jpg" alt="Card image cap"/>
                                <div className="card-body">
                                    <h2 className="card-title">Adventure tour around Brazil</h2>
                                    <p className="card-text">Best-selling Brazil tour! All the best things to do in the summer with our first-rate guide Ricardo!</p>
                                    <a href="#" className="btn waves-effect waves-light #81c784 green lighten-2">Read More &rarr;</a>
                                </div>
                                <div className="card-footer text-muted">
                                    Posted on October 27, 2019 by
                                    &nbsp;<a href="#">Some company</a>
                                </div>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
        )
    }
}

/*
http://placehold.it/750x300
<div className="container">
    <div className="row h3">
        Company Employees
    </div>
    <div className="row">
        <div className="col-8 col-md-5">
            <div>
                <div className="panel panel-default pt-2 pl-2 pb-2">
                    <div className="panel-heading">
                        <div className="panel-title h5">Company Guides</div>
                    </div>
                    <div className="panel-body">
                        <div className="media">
                            <div className="media-left mr-2">
                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                            </div>
                            <div className="media-body pb-2">
                                <div className="text-bold">Floyd Ortiz
                                    <div className="text-sm text-muted">12m ago</div>
                                </div>
                            </div>
                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                        </div>
                        <div className="media">
                            <div className="media-left mr-2">
                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                            </div>
                            <div className="media-body pb-2">
                                <div className="text-bold">Luis Vasquez
                                    <div className="text-muted">2h ago</div>
                                </div>
                            </div>
                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                        </div>
                        <div className="media">
                            <div className="media-left mr-2">
                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                            </div>
                            <div className="media-body pb-2">
                                <div className="text-bold">Duane Mckinney
                                    <div className="text-muted">yesterday</div>
                                </div>
                            </div>
                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                        </div>
                        <div className="media">
                            <div className="media-left mr-2">
                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                            </div>
                            <div className="media-body pb-2">
                                <div className="text-bold">Connie Lambert
                                    <div className="text-muted">2 weeks ago</div>
                                </div>
                            </div>
                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div className="col-4 col-md-5">
            <div className="btn-group-vertical mr-2" role="group" aria-label="First group">
                <button type="button" className="btn waves-effect waves-light #81c784 green lighten-2 mb-2">Add Guid</button>
                <button type="button" className="btn waves-effect waves-light #81c784 green lighten-2">Add Admin</button>
            </div>
        </div>



        <div className="col-8 col-md-5">
            <div>
                <div className="panel panel-default pt-2 pl-2 pb-2">
                    <div className="panel-heading">
                        <div className="panel-title h5">Company Administrators</div>
                    </div>
                    <div className="panel-body">
                        <div className="media">
                            <div className="media-left mr-2">
                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                            </div>
                            <div className="media-body pb-2">
                                <div className="text-bold">Floyd Ortiz
                                    <div className="text-muted">12m ago</div>
                                </div>
                            </div>
                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                        </div>
                        <div className="media">
                            <div className="media-left mr-2">
                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                            </div>
                            <div className="media-body pb-2">
                                <div className="text-bold">Luis Vasquez
                                    <div className="text-muted">2h ago</div>
                                </div>
                            </div>
                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                        </div>
                        <div className="media">
                            <div className="media-left mr-2">
                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                            </div>
                            <div className="media-body pb-2">
                                <div className="text-bold">Duane Mckinney
                                    <div className="text-muted">yesterday</div>
                                </div>
                            </div>
                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                        </div>
                        <div className="media">
                            <div className="media-left mr-2">
                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                            </div>
                            <div className="media-body pb-2">
                                <div className="text-bold">Connie Lambert
                                    <div className="text-muted">2 weeks ago</div>
                                </div>
                            </div>
                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>*/
