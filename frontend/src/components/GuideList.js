import React, { PureComponent } from 'react';
import { ToastContainer, toast } from 'react-toastify';

export default class GuideList extends PureComponent {
    constructor(props) {
        super(props);
        this.state = {}
    }


    render() {
        return (
            <>
            <div className="container">
                <div className="row mt-5">
                    <div className="col-12 h3 text-center mt-5">
                        Find a guide
                    </div>
                </div>
                <div className="row justify-content-center">
                    <div className="col-8 h5 text-center">
                        You can find a guide by typing name, email of the guide or enter some key words.
                    </div>
                </div>
                <div className="row justify-content-center mt-4">
                    <div className="col-8">
                        <div className="form-group">
                            <label htmlFor="guide-info"></label>
                            <input ref="description" type="text" className="form-control" id="guide-info" placeholder="Enter search terms" />
                        </div> 
                    </div>
                </div>
                <div className="row justify-content-center text-center mt-3">
                    <div className="col-8">
                    <a className="btn waves-effect waves-light #81c784 green lighten-2">Find a guide</a>
                    </div>
                </div>
                <div className="row justify-content-center">
                    <div className="col-8">
                        <div className="panel-body mt-5">
                            <div className="media">
                                <div className="media-left mr-2">
                                    <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact" /></a>
                                </div>
                                <div className="media-body pb-2">
                                    <div className="text-bold">Guide Name
                                        <div className="text-sm text-muted">Time</div>
                                    </div>
                                </div>
                                <a className="btn waves-effect waves-light #81c784 green lighten-2">Add guide</a>
                            </div>

                            <div className="media">
                                <div className="media-left mr-2">
                                    <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact" /></a>
                                </div>
                                <div className="media-body pb-2">
                                    <div className="text-bold">Guide Name
                                        <div className="text-sm text-muted">Time</div>
                                    </div>
                                </div>
                                <a className="btn waves-effect waves-light #81c784 green lighten-2">Add guide</a>
                            </div>

                            <div className="media">
                                <div className="media-left mr-2">
                                    <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact" /></a>
                                </div>
                                <div className="media-body pb-2">
                                    <div className="text-bold">Guide Name
                                        <div className="text-sm text-muted">Time</div>
                                    </div>
                                </div>
                                <a className="btn waves-effect waves-light #81c784 green lighten-2">Add guide</a>
                            </div>
                        </div>
                                
                    </div>
                </div>
            </div>
            </>
        )
    }
}