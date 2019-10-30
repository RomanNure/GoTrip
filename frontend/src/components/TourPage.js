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
                <div className="container mt-5">
                    <div className="row justify-content-center">
                        <div className="col-12 text-center h3 mt-5">
                            Tour Name
                        </div>
                        <div className="col-12">
                            <div className="head-image-wrapper">
                                <img src="/images/bestplaceholder.jpg" className="img-thumbnail mr-auto ml-auto" alt=""/>
                            </div>
                        </div>
                    </div>
                    <div className="row image-wrapper">
                        <div className="col-12 col-lg-4 mb-4">
                            <img src="/images/bestplaceholder.jpg" className="img-thumbnail mx-auto d-block" alt=""/>
                        </div>
                        <div className="col-12 col-lg-4 mb-4">
                            <img src="/images/bestplaceholder.jpg" className="img-thumbnail mx-auto d-block" alt=""/>
                        </div>
                        <div className="col-12 col-lg-4 mb-4">
                            <img src="/images/bestplaceholder.jpg" className="img-thumbnail mx-auto d-block" alt=""/>
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
                </div>
            </>
        )
    }
}
