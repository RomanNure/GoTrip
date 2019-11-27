import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios'
import ReactModal from 'react-modal';


export default class EmployeeList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            modal: false
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
                    i.then(d => users.push(d.data))
                })
                //console.log('users', users)

                this.state.administrators = users
                this.setState()
                this.forceUpdate()
            }));
    }
   
    _onSentRequest = () => {
        this.setState({ modal: false })
        this.props._onAddAdmin(this.refs.admin.value) 
    }

    render() {
        console.log('this.props=>', this.props)
        let { administrators } = this.state
        console.log('this.state.', this.state)
        console.log('administrators=>', administrators && administrators.length)
        return (
            <div className="container">
                <ReactModal
                    isOpen={this.state.modal}
                    style={{
                        overlay: {
                            backgroundColor: "inharit"
                        },
                        content: {
                            marginLeft: "35%",
                            marginTop: "10%",
                            marginBottom: "20%",
                            alignItems: "space-between",
                            width: "30%",
                            borderRadius: 30,
                            color: 'lightsteelblue'
                        }
                    }}
                >
                    <div style={{ marginLeft: "30%" }}>
                        <h2>
                            Add Admin
                        </h2>
                    </div>
                    <input style={{ marginTop: "10%" }} ref="admin" type="text" placeholder="login/email of admin" disabled={false/*!this.state.rule*/} />
                    <a  className="btn waves-effect waves-light #81c784 black lighten-2" onClick={() => this.setState({modal:false})}>close</a>

                    <a style={{ marginLeft: "70%", marginTop: "8%" }} className="btn waves-effect waves-light #81c784 green lighten-2"
                        onClick={() => this._onSentRequest}>Add admin</a>

                </ReactModal>
                <div className="row h3 m-4">
                    Company Employees
                </div>
                <div className="row">

                    <div className="col-8 col-md-9">
                        <div>
                            <div className="panel panel-default pt-2 pl-2 pb-2">
                                <div className="panel-heading">
                                    <div className="panel-title h5">Company Administrators</div>
                                </div>
                                {administrators && administrators.length > 0 ?
                                    administrators.map(({ id, login, fullName, time, avatarUrl }, i) => {
                                        console.log('admin=>', i, administrators[i])
                                        return <div className="media" key={i} onClick={() => this.props.history.push('/user:' + id)}>
                                            <div className="media-left mr-2">
                                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src={avatarUrl ? avatarUrl : "images/Avatar.png"} alt="Contact" /></a>
                                            </div>
                                            <div className="media-body pb-2">
                                                <div className="text-bold">{fullName ? fullName : login}
                                                    <div className="text-sm text-muted">{time}</div>
                                                </div>
                                            </div>
                                            <i className="material-icons mt-2 mr-3 icon-red">highlight_off</i>
                                        </div>
                                    })
                                    :
                                    <div>No Administrators yet :(</div>
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div >
        )
    }
}
