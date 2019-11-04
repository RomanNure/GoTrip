import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios'

export default class CompanyPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            /*guides: [
                {
                    id: 4,
                    name: "Ricardo",
                    time: "active",
                    avatarUrl: "./images/bestplaceholder.jpg"
                }
            ],
            administrators: [
                {
                    id: 4,
                    name: "Ricardo",
                    time: "active",
                    avatarUrl: "./images/bestplaceholder.jpg"
                }
            ]*/
        }
        this._getUsers = this._getUsers.bind(this)
    }
    componentDidMount() {
        this._getUsers()
    }
    _getUsers = async () => {
        let options = {
            method: "get",
            url: 'https://go-trip.herokuapp.com/user/get?id=',
            headers: {
                'Content-Type': 'application/x-www-from-urlencoded',//Content-Type': 'appication/json',
            },
        }

        let { administrators } = this.props
        let ids = administrators.map(i => {
            let url = JSON.parse(JSON.stringify(options))
            url.url += i.id
            return axios(url)
        })
        axios.all([ids])
            .then(axios.spread((acct, perms) => {
                let users = []
                acct.map(i => {
                    i.then(d => console.log('then =>>>>>', users.push(d.data)))
                })
                console.log('users', users)

                this.setState({ administrators: users })
                // Both requests are now complete
            }));
        /*let d = ids.map( i => {
            axios()
                .then(({ data }) => {
                    console.log('data=>', data)
                    return data
                    /*    let { email, login, phone, fullName, description, avatarUrl } = data
                        let rule = (this.state.user && login == this.state.user.login) ? true : false
                        let user = cookie.load('user')
                        if (rule && avatarUrl) {
                            user.avatarUrl = avatarUrl
                            cookie.save('user', user, { path: '/' })
                        }
                        this.setState({ email, login, phone, fullName, rule, description, avatarUrl })
                        // if (rule) window.location.reload();
    
                })
        })
        console.log('d=>', d)
      */  console.log('ids', ids)
    }


    render() {
        console.log('this.props=>', this.props)
        let { administrators } = this.state
        console.log('this.state.', this.state)
        console.log('admins=>', administrators)
        return (
            <div className="container">
                <div className="row h3 m-4">
                    Company Employees
                </div>
                <div className="row">
                    <div className="col-7 col-md-9">
                        <div>
                            <div className="panel panel-default pt-2 pl-2 pb-2">
                                <div className="panel-heading">
                                    <div className="panel-title h5">Company Guides</div>
                                </div>
                                <div className="panel-body">
                                    {this.state.guides && this.state.guides.length > 0 ?
                                        this.state.guides.map(({ id, name, time, avatarUrl }, i) => {
                                            return <div className="media" key={i} onClick={() => this.props.history.push('/user:' + id)}>
                                                <div className="media-left mr-2">
                                                    <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src={avatarUrl ? avatarUrl : "images/Avatar.png"} alt="Contact" /></a>
                                                </div>
                                                <div className="media-body pb-2">
                                                    <div className="text-bold">{name}
                                                        <div className="text-sm text-muted">{time}</div>
                                                    </div>
                                                </div>
                                                <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                            </div>
                                        })
                                        :
                                        <div>No Guides yet :(</div>
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-2 col-md-3" style={{ right: 0 }}>
                        <div className="btn-group-vertical mr-2" role="group" aria-label="First group">
                            <a type="button" className="btn waves-effect waves-light #81c784 green lighten-2 mb-2">Add Guid</a>
                            <a type="button" className="btn waves-effect waves-light #81c784 green lighten-2" onClick={this.props._onAddAdmin}>Add Admin</a>
                        </div>
                    </div>

                    <div className="col-8 col-md-9">
                        <div>
                            <div className="panel panel-default pt-2 pl-2 pb-2">
                                <div className="panel-heading">
                                    <div className="panel-title h5">Company Administrators</div>
                                </div>
                                {administrators && administrators.length > 0 ?
                                    administrators.map(({ id, name, time, avatarUrl }, i) => {
                                        return <div className="media" key={i} onClick={() => this.props.history.push('/user:' + id)}>
                                            <div className="media-left mr-2">
                                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src={avatarUrl ? avatarUrl : "images/Avatar.png"} alt="Contact" /></a>
                                            </div>
                                            <div className="media-body pb-2">
                                                <div className="text-bold">{name}
                                                    <div className="text-sm text-muted">{time}</div>
                                                </div>
                                            </div>
                                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                        </div>
                                    })
                                    :
                                    <div>No Administrators yes :(</div>
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div >
        )
    }
}
