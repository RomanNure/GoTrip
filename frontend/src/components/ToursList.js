import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export default class ToresList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tours: this.props.tours ? this.props.tours : [
                {
                    img: "./images/placeholder.jpg",
                    title: "Adventure tour around Brazil",
                    descr: "Best-selling Brazil tour! All the best things to do in the summer with our first-rate guide Ricardo!",
                    time: "October 27, 2019"
                },
                {
                    img: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.telegraph.co.uk%2Fcontent%2Fdam%2Fnews%2F2018%2F04%2F18%2FTELEMMGLPICT000157926081-xlarge_trans_NvBQzQNjv4BqvxY1SBh3Zy94n8Z2-u3DXpo3vSb9RvelYMC6seL5330.jpeg&f=1&nofb=1",
                    title: "Adventure tour to mars",
                    descr: "Mask v woke",
                    time: "October 29, 2019"
                }, {
                    img: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.telegraph.co.uk%2Fcontent%2Fdam%2Fnews%2F2018%2F04%2F18%2FTELEMMGLPICT000157926081-xlarge_trans_NvBQzQNjv4BqvxY1SBh3Zy94n8Z2-u3DXpo3vSb9RvelYMC6seL5330.jpeg&f=1&nofb=1",
                    title: "Adventure tour to mars",
                    descr: "Mask v woke",
                    time: "October 29, 2019"
                }, {
                    img: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.telegraph.co.uk%2Fcontent%2Fdam%2Fnews%2F2018%2F04%2F18%2FTELEMMGLPICT000157926081-xlarge_trans_NvBQzQNjv4BqvxY1SBh3Zy94n8Z2-u3DXpo3vSb9RvelYMC6seL5330.jpeg&f=1&nofb=1",
                    title: "Adventure tour to mars",
                    descr: "Mask v woke",
                    time: "October 29, 2019"
                }, {
                    img: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.telegraph.co.uk%2Fcontent%2Fdam%2Fnews%2F2018%2F04%2F18%2FTELEMMGLPICT000157926081-xlarge_trans_NvBQzQNjv4BqvxY1SBh3Zy94n8Z2-u3DXpo3vSb9RvelYMC6seL5330.jpeg&f=1&nofb=1",
                    title: "Adventure tour to mars",
                    descr: "Mask v woke",
                    time: "October 29, 2019"
                },
            ]
        }
    }


    render() {
        console.log('rendered toursList')
        return (
            <div style={{ display: "flex", flexDirection: "row", justifyContent: "flex-start", alignItems: "center", flexWrap: "wrap" }}>
                {this.state.tours && this.state.tours.length > 0 ?
                    (<>
                        {this.state.tours.map(({ descr, title, img, time }) => {
                            return <div style={{ width: "40%", margin:"50px", marginTop:"5px", marginLeft:"auto", marginRight:"auto" }}>
                                <div className="card rounded mb-4">
                                    <img className="card-img-top" src={img} alt="Card image cap" style={{ height: 300 }} />
                                    <div className="card-body">
                                        <h2 className="card-title">{title}</h2>
                                        <p className="card-text">{descr}</p>
                                        <a href="#" className="btn waves-effect waves-light #81c784 green lighten-2">Read More &rarr;</a>
                                    </div>
                                    <div className="card-footer text-muted">
                                        Posted on {time} by
                                                &nbsp;<a href="#">Some company</a>
                                    </div>
                                </div>
                            </div>

                        })
                        }
                    </>)
                    :
                    <div className="mx-auto text-center" style={{ fontSize: 24 }}>No Tours yet :(</div>
                }
            </div>
        )
    }
}

/*

                            <div className="row">
                                <div className="col-md-8" >
                                    <div className="card rounded mb-4">
                                        <img className="card-img-top" src={img} alt="Card image cap" style={{ height: 300 }} />
                                        <div className="card-body">
                                            <h2 className="card-title">{title}</h2>
                                            <p className="card-text">{descr}</p>
                                            <a href="#" className="btn waves-effect waves-light #81c784 green lighten-2">Read More &rarr;</a>
                                        </div>
                                        <div className="card-footer text-muted">
                                            Posted on {time} by
                                                &nbsp;<a href="#">Some company</a>
                                        </div>
                                    </div>
                                </div>
                            </div>







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
