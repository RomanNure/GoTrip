import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios'
import ReactModal from 'react-modal';
import { getCompanyAdmins } from '../api.js'

export default class EmployeeList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            modal: false
        }
        this._getUsers()
    }
    componentDidMount() {

    }

    _getUsers = async () => {
        /*  let options = {
              method: "get",
              url: 'https://go-trip.herokuapp.com/user/get?id=',
              headers: {
                  'Content-Type': 'application/x-www-from-urlencoded',//Content-Type': 'appication/json',
              },
          }
  
          let { administrators } = this.props
          administrators.push({ id: 106 })
          let ids = administrators.map(i => {
              let url = JSON.parse(JSON.stringify(options))
              url.url += i.id
              return axios(url)
          })
          //console.log("ids=>",ids)
          axios.all([ids])
              .then(axios.spread((acct, perms) => {
                  let users = []
                  acct.map(i => {
                      i.then(d => users.push(d.data))
                  })
                  console.log('users', users)
  
                  this.state.administrators = users
                  this.setState()
                  this.forceUpdate()
              }));*/
        getCompanyAdmins(this.props.id).then(({ data }) => this.setState({ admins: data }))
    }

    _onSentRequest = () => {
        this.setState({ modal: false })
        this.props._onAddAdmin(this.refs.admin.value)
    }

    render() {
        console.log('this.props=>', this.props)
        let { admins } = this.state
        console.log('admins => >> > >', admins)
        // console.log('administrators=>', administrators && administrators.length)
        return (
            <div className="container">
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
                                    admins.map(({ id, login, fullName, email, avatarUrl }, i) => {
                                        // console.log('admin=>', i, administrators[i])
                                        return <div className="media" key={i} onClick={() => this.props.history.push('/user:' + id)}>
                                            <div className="media-left mr-2">
                                                <a href="#"><img className="media-object rounded-circle img-thumbnail thumb48" src={avatarUrl ? avatarUrl : "images/Avatar.png"} alt="Contact" /></a>
                                            </div>
                                            <div className="media-body pb-2">
                                                <div className="text-bold">{fullName ? fullName : login}
                                                    <div className="text-sm text-muted">{email}</div>
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
