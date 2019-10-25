import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export default class CompanyPage extends Component {
    constructor(props) {
        super(props);
        this.state = { }
    }


    render() {
        return (
            <div>
                <div className="container">
                    <div className="panel panel-default col-md-7 ml-auto mr-auto pb-2 pt-2 pl-4 pr-4">
                        <div className="row text-center">
                            <div className="col-12 text-center">
                                <div className="h3">Create your company</div>
                            </div>
                        </div>
                        <div className="row justify-content-center">
                            <div className="col-12">
                                <form action="">
                                    <div className="form-group">
                                        <label htmlFor="company-name">Company name</label>
                                        <input type="text" className="form-control" id="company-name" placeholder="Enter company name"/>
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="company-email">Company email</label>
                                        <input type="email" className="form-control" id="company-email" placeholder="Enter email"/>
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="company-website">Company website</label>
                                        <input type="text" className="form-control" id="company-website" placeholder="Website URL"/>
                                    </div>
                                    <button type="submit" className="btn waves-effect waves-light #81c784 green lighten-2">Create Company</button>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
