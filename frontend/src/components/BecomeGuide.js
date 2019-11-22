import React, { PureComponent } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import { becomeGuide } from '../api.js'
import GlobalContext from '../GlobalContext';

export default class BecomeGuidePage extends PureComponent {
    static contextType = GlobalContext
    constructor(props) {
        super(props);
        this.state = {}
    }
    componentDidMount() {
        console.log("context guide =>", this.context)
        if (!this.context.user.id)
            this.props.history.push('/login')
    }

    _onSubmit = async () => {
        const { card: { value: cValue }, words: { value: wValue } } = this.refs
        const CARD = /[\d\-]{16,20}/
        if (!cValue) {
            toast.error("Please type your card number !", {
                position: toast.POSITION.TOP_RIGHT
            });
        }
        if (!wValue) {
            toast.error("Please type keywords !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        if (!CARD.test(cValue)) {
            toast.error("Invalid card number !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }

        let wantedToursKeyWords = wValue.split(/[\s\,\.\/\\]+/img).join(',')
        becomeGuide({ idRegisteredUser: this.context.id, cardNumber: cValue, wantedToursKeyWords })

        //console.log("w tour key=>", wantedToursKeyWords)

        //console.log('card! =>', cardNumber, wantedToursKeyWords)
    }

    render() {
        return (
            <>
                <ToastContainer />

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
                                <input ref="card" type="text" className="form-control" id="bank-info" placeholder="Enter your credentials" />
                            </div>
                        </div>
                    </div>
                    <div className="row justify-content-center">
                        <div className="col-8">
                            <div className="form-group">
                                <label htmlFor="key-words">Key words</label>
                                <input ref="words" type="text" className="form-control" id="key-words" placeholder="Enter key words" />
                            </div>
                        </div>
                    </div>
                    <div className="row justify-content-center text-center mt-5">
                        <div className="col-8">
                            <a className="btn waves-effect waves-light #81c784 green lighten-2" onClick={this._onSubmit}>Become a guide</a>
                        </div>
                    </div>
                </div>
            </>
        )
    }
}