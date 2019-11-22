import React, { PureComponent } from 'react';

export default class Footer extends PureComponent {
    constructor(props) {
        super(props);
    }
    shouldComponentUpdate(nextProps, nextState) {
        return false
    }


    render() {
        return (
            <footer className="page-footer #81c784 green lighten-2 bt-0" style={{ }}>
                <div className="container" style={{ height: 50 }}>
                    <div className="row">
                        <div className="col l6 s12">
                            <p className="grey-text text-lighten-4">Make your dream come true with us!</p>
                        </div>
                        <div className="col l4 offset-l2 s12">
                            <h5 className="white-text">Contact us</h5>
                            <ul>
                                <li><a className="grey-text text-lighten-3" href="#!">go.trip.supp@gmail.com</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div className="footer-copyright">
                    <div className="container">
                        © 2019 Go&Trips
                        <a className="grey-text text-lighten-4 right" href="#!">More Links</a>
                    </div>
                </div>
            </footer>
        )
    }
}
