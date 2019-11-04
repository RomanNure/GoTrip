import React, { PureComponent } from 'react';
import { ToastContainer, toast } from 'react-toastify';

export default class TourPage extends PureComponent {
    constructor(props) {
        super(props);
        this.state = {}
    }


    render() {
        return (
            <>
                <div className="container mt-2">
                    <div className="row justify-content-center">
                        <div className="col-12 text-center h3 mt-5">
                            Tour Name
                        </div>
                        <div className="col-12">
                            <div className="head-image-wrapper">
                                <img src="/images/placeholder.jpg" className="img-thumbnail mr-auto ml-auto" alt=""/>
                            </div>
                        </div>
                    </div>
                    <div className="row image-wrapper">
                        <div className="col-12 col-lg-4 mb-4">
                            <img src="/images/placeholder.jpg" className="img-thumbnail mx-auto d-block" alt=""/>
                        </div>
                        <div className="col-12 col-lg-4 mb-4">
                            <img src="/images/placeholder.jpg" className="img-thumbnail mx-auto d-block" alt=""/>
                        </div>
                        <div className="col-12 col-lg-4 mb-4">
                            <img src="/images/placeholder.jpg" className="img-thumbnail mx-auto d-block" alt=""/>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-12 text-center h4">
                            Description
                        </div>
                        <div className="col-12">
                            <div>
                               Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                                Aliquid animi assumenda at atque aut consectetur consequuntur cum deleniti eaque eos esse fugit harum impedit laudantium molestiae neque odio quae, quis sed sequi similique ullam voluptatum.
                                Delectus error provident recusandae ut.
                                Accusamus deserunt doloremque eos molestiae nesciunt quo totam ullam voluptatum.
                            </div>
                        </div>
                    </div>

                    <div className="panel panel-default pl-4 pt-3 pr-4 pb-3 mt-5">
                        <div className="row text-center">
                            <div className="col-12 h4">
                                Tour Info
                            </div>
                        </div>
                        <div className="row justify-content-center">
                            <div className="col-10">
                                <form className="">
                                    <div className="form-group">
                                        <label htmlFor="tour-price">Tour price</label>
                                        <input ref="" type="text" className="form-control" id="tour-price" placeholder="Tour price" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="tour-start">Start date</label>
                                        <input ref="" type="text" className="form-control" id="tour-start" placeholder="Start" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="tour-end">End date</label>
                                        <input ref="" type="text" className="form-control" id="tour-end" placeholder="End" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="tour-duration">Tour duration</label>
                                        <input ref="" type="text" className="form-control" id="tour-duration" placeholder="Duration" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="tour-places">Places</label>
                                        <input ref="" type="text" className="form-control" id="tour-places" placeholder="Places" />
                                    </div>
                                    <div className="form-group">
                                        <a className="btn waves-effect waves-light #81c784 green lighten-2">Update</a>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>


                    <div className="row mt-4">
                        <div className="col-12">

                            <div className="media">
                                <div className="media-left mr-2">
                                    <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact" /></a>
                                </div>
                                <div className="media-body pb-2">
                                    <div className="text-bold">Company name
                                        <div className="text-sm text-muted">Holder company</div>
                                    </div>
                                </div>
                                <a className="btn waves-effect waves-light #81c784 green lighten-2">Change</a>
                            </div>

                            <div className="media">
                                <div className="media-left mr-2">
                                    <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact" /></a>
                                </div>
                                <div className="media-body pb-2">
                                    <div className="text-bold">Admin name
                                        <div className="text-sm text-muted">Administrator</div>
                                    </div>
                                </div>
                                <a className="btn waves-effect waves-light #81c784 green lighten-2">Change</a>
                            </div>

                            <div className="media">
                                <div className="media-left mr-2">
                                    <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src="images/Avatar.png" alt="Contact" /></a>
                                </div>
                                <div className="media-body pb-2">
                                    <div className="text-bold">Guid name
                                        <div className="text-sm text-muted">Guid</div>
                                    </div>
                                </div>
                                <a className="btn waves-effect waves-light #81c784 green lighten-2">Change</a>
                            </div>
                        </div>

                    </div>


                </div>
            </>
        )
    }
}
