import React, { Component } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import { getCompanyAdmins, removeArdimistrator } from '../api.js'

export default class EmployeeList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            modal: false
        }
        this._getUsers()
    }

    _getUsers = async () => {
        getCompanyAdmins(this.props.id).then(({ data }) => this.setState({ admins: data }))
    }

    _onRemoveAdmin = (id) => {
        console.log("remove admin id=>", id)
        removeArdimistrator(id)
            .then(data => this._getUsers)
            .catch(error => {
                toast.error(error.response.data.message, {
                    position: toast.POSITION.TOP_RIGHT
                });
            })
    }

    render() {
        console.log('this.props=>', this.props)
        let { admins } = this.state
        console.log('admins => >> > >', admins)
        // console.log('administrators=>', administrators && administrators.length)
        return (
            <div className="container">
                <ToastContainer />

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
                                {admins && admins.length > 0 ?
                                    admins.map(({ id,administratorId, login, fullName, email, avatarUrl }, i) => {
                                        // console.log('admin=>', i, administrators[i])
                                        return <div className="media" key={i} >
                                            <div className="media-left mr-2" onClick={() => this.props.history.push('/user:' + id)}>
                                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src={avatarUrl ? avatarUrl : "images/Avatar.png"} alt="Contact" /></a>
                                            </div>
                                            <div className="media-body pb-2" onClick={() => this.props.history.push('/user:' + id)} >
                                                <div className="text-bold">{fullName ? fullName : login}
                                                    <div className="text-sm text-muted">{email}</div>
                                                </div>
                                            </div>
                                            <i className="material-icons mt-2 mr-3 icon-red" style={{ cursor: "pointer" }} onClick={() => this._onRemoveAdmin(administratorId)}>highlight_off</i>
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
