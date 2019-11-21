import React, { PureComponent } from 'react';
import { ToastContainer, toast } from 'react-toastify';

export default class BecomeGuide extends PureComponent {
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
                        Become a guide
                    </div>
                </div>
                <div className="row justify-content-center">
                    <div className="col-8 h5 text-center">
                        To become a guid you need to enter your bank credentials and some key words to make easy for anyone to find you.
                    </div>
                </div>
                <div className="row justify-content-center mt-4">
                    <div className="col-8">
                        <div className="form-group">
                            <label htmlFor="bank-info">Bank credentials</label>
                            <input ref="description" type="text" className="form-control" id="bank-info" placeholder="Enter your credentials" />
                        </div> 
                    </div>
                </div>
                <div className="row justify-content-center">
                    <div className="col-8">
                        <div className="form-group">
                            <label htmlFor="key-words">Key words</label>
                            <input ref="description" type="text" className="form-control" id="key-words" placeholder="Enter key words" />
                        </div> 
                    </div>
                </div>
                <div className="row justify-content-center text-center mt-5">
                    <div className="col-8">
                    <a className="btn waves-effect waves-light #81c784 green lighten-2">Become a guide</a>
                    </div>
                </div>
            </div>
            </>
        )
    }
}