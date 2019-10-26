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
            <footer className="page-footer #81c784 green lighten-2" style={{ bottom: 0 }}>
                <div className="container">
                    <div className="row">
                        <div className="col l6 s12">
                            <h5 className="white-text">Footer Content</h5>
                            <p className="grey-text text-lighten-4">You can use rows and columns here to organize your footer content.</p>
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
