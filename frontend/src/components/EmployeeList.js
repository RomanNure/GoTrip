import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export default class CompanyPage extends Component {
    constructor(props) {
        super(props);
        this.state = {}
    }


    render() {
        return (
            <div className="container">
                <div className="row h3">
                    Company Employees
                </div>
                <div className="row">
                   <div className="col-5">
                       <div>
                           <div className="panel panel-default hidden-xs hidden-sm pt-2 pl-3 pb-2">
                               <div className="panel-heading">
                                   <div className="panel-title h5">Company Guides</div>
                               </div>
                               <div className="panel-body">
                                   <div className="media">
                                       <div className="media-left media-middle mr-2">
                                           <a href="#"><img className="media-object img-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                                       </div>
                                       <div className="media-body pb-2">
                                           <div className="text-bold">Floyd Ortiz
                                               <div className="text-sm text-muted">12m ago</div>
                                           </div>
                                       </div>
                                       <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                   </div>
                                   <div className="media">
                                       <div className="media-left media-middle mr-2">
                                           <a href="#"><img className="media-object img-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                                       </div>
                                       <div className="media-body pb-2">
                                           <div className="text-bold">Luis Vasquez
                                               <div className="text-sm text-muted">2h ago</div>
                                           </div>
                                       </div>
                                       <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                   </div>
                                   <div className="media">
                                       <div className="media-left media-middle mr-2">
                                           <a href="#"><img className="media-object img-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                                       </div>
                                       <div className="media-body pb-2">
                                           <div className="text-bold">Duane Mckinney
                                               <div className="text-sm text-muted">yesterday</div>
                                           </div>
                                       </div>
                                       <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                   </div>
                                   <div className="media">
                                       <div className="media-left media-middle mr-2">
                                           <a href="#"><img className="media-object img-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                                       </div>
                                       <div className="media-body pb-2">
                                           <div className="text-bold">Connie Lambert
                                               <div className="text-sm text-muted">2 weeks ago</div>
                                           </div>
                                       </div>
                                       <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                   </div>
                               </div>
                           </div>
                       </div>
                   </div>
                    <div className="col-5">
                        <div className="btn-group-vertical mr-2" role="group" aria-label="First group">
                            <button type="button" className="btn waves-effect waves-light #81c784 green lighten-2 mb-2">Add Guid</button>
                            <button type="button" className="btn waves-effect waves-light #81c784 green lighten-2">Add Administrator</button>
                        </div>
                    </div>



                    <div className="col-5">
                        <div>
                            <div className="panel panel-default hidden-xs hidden-sm pt-2 pl-3 pb-2">
                                <div className="panel-heading">
                                    <div className="panel-title h5">Company Administrators</div>
                                </div>
                                <div className="panel-body">
                                    <div className="media">
                                        <div className="media-left media-middle mr-2">
                                            <a href="#"><img className="media-object img-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                                        </div>
                                        <div className="media-body pb-2">
                                            <div className="text-bold">Floyd Ortiz
                                                <div className="text-sm text-muted">12m ago</div>
                                            </div>
                                        </div>
                                        <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                    </div>
                                    <div className="media">
                                        <div className="media-left media-middle mr-2">
                                            <a href="#"><img className="media-object img-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                                        </div>
                                        <div className="media-body pb-2">
                                            <div className="text-bold">Luis Vasquez
                                                <div className="text-sm text-muted">2h ago</div>
                                            </div>
                                        </div>
                                        <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                    </div>
                                    <div className="media">
                                        <div className="media-left media-middle mr-2">
                                            <a href="#"><img className="media-object img-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                                        </div>
                                        <div className="media-body pb-2">
                                            <div className="text-bold">Duane Mckinney
                                                <div className="text-sm text-muted">yesterday</div>
                                            </div>
                                        </div>
                                        <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                    </div>
                                    <div className="media">
                                        <div className="media-left media-middle mr-2">
                                            <a href="#"><img className="media-object img-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact"/></a>
                                        </div>
                                        <div className="media-body pb-2">
                                            <div className="text-bold">Connie Lambert
                                                <div className="text-sm text-muted">2 weeks ago</div>
                                            </div>
                                        </div>
                                        <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
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
