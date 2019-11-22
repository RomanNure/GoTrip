import React, { Component } from 'react';
import ReactModal from 'react-modal';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import cookie from 'react-cookies'
import { getUser, getCompanyOwner, updateUser, addUserPhoto } from '../api';
//import formidable from 'formidable'

export default class UserPage extends Component {
    constructor(props) {
        super(props);
        let { state } = this.props.location
        this.state = state

        if (!this.state) this.state = {
            rule: false,
            modal: false
        }
        this.state.user = cookie.load("user")
        this.state.id = this.props.location.pathname.match(/\:\d+/)[0].substr(1)
        //console.log('id', this.state.id)
    }

    componentDidMount() {
        //console.log('this.tate=>', this.state)
        if (!this.state.login && this.state.id) {

            getUser(this.state.id).then(({ data }) => {
                //console.log(' user data=>', data)
                if (!data.email) return false
                return data
            })
                .catch(error => {
                    toast.error('server not response', {
                        position: toast.POSITION.TOP_RIGHT
                    });
                    console.log('Error', error);
                })
                .then(userData => {
                    getCompanyOwner(this.state.id).then(company => {
                        if (!userData) throw "err"

                        let { email, login, phone, fullName, description, avatarUrl, guide, administrators } = userData
                        let rule = (this.state.user && login == this.state.user.login) ? true : false
                        let user = cookie.load('user')
                        if (rule && avatarUrl) {
                            user.avatarUrl = avatarUrl
                            cookie.save('user', user, { path: '/' })
                        }
                        this.setState({ email, login, phone, fullName, rule, description, avatarUrl, company: company.data[0], guide, administrators })
                    })
                })
                .catch(err => {
                    toast.error('error=>' + err.toString(), {
                        position: toast.POSITION.TOP_RIGHT
                    });
                })
        }
    }


    _onUpdate = (data) => {
        if (!this.state.rule) return
        let { fullName, phone, email, description } = this.refs
        let NAME = /\w{1,20}\s{1}\w{1,20}/ig
        let PHONE = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im
        if (!fullName.value) {
            toast.error('Please type your name !', { position: toast.POSITION.TOP_RIGHT });
        }
        if (!phone.value) {
            toast.error("Please type your phone !", { position: toast.POSITION.TOP_RIGHT });
            return
        }

        if (NAME.test(fullName.value)) {
            console.log('email, phone', fullName, phone);
        } else {
            toast.error('Invalid name !', { position: toast.POSITION.TOP_RIGHT });
            return
        }

        if (PHONE.test(phone.value)) {
            console.log('email, phone', fullName, phone);
        } else {
            toast.error('Invalid phone !', { position: toast.POSITION.TOP_RIGHT });
            return
        }
        //console.log('this.state.avatar.url', this.state.avatarUrl.toString(), data)
        let avatarUrl = data ? "http://185.255.96.249:5000/GoTrip/GoTripImgs/avatars/" + data : this.state.avatarUrl
        let user = {
            id: this.state.id,
            phone: phone.value,
            fullName: fullName.value,
            login: this.state.user.login,
            password: this.state.user.password,
            email: email.value,
            description: description ? description.value : null,
            avatarUrl
        }
        //user.avatarUrl = "http://185.255.96.249:5000/GoTrip/GoTripImgs/avatars/5.png"
        console.log('this.state.avatarUrl', user.avatarUrl)
        updateUser(user).then(({ data }) => {
            toast.success("Updated", { position: toast.POSITION.TOP_RIGHT });
            this.props.history.push({ pathname: '/user:' + data.id, state: data })//, {props: data})
            // append to DOM
        })
            .catch(error => {
                if (error.response) {
                    toast.error(error.response.data.message, { position: toast.POSITION.TOP_RIGHT });
                } else if (error.request) {
                    console.log('request err', error.request);
                } else {
                    console.log('Error', error.message);
                }
                console.log('Error', error);
            });

    }
    _onUploadPhoto = () => (e) => {
        e.preventDefault()
        const file = e.target.files[0];
        let type = file.type.split('/')[1];
        var formData = new FormData();
        formData.append('file', file, this.state.id + "." + type);
        addUserPhoto(formData).then(data => {
            toast.success('Photo updated', { position: toast.POSITION.TOP_RIGHT });
        })
            .catch(error => {
                if (error.response) {
                    toast.error(error.response.data.message, { position: toast.POSITION.TOP_RIGHT });
                }
                console.log('Error', error);
            })
        this._onUpdate(this.state.id + '.' + type)
    }

    _onLogOut = () => {
        cookie.remove('user', { path: '/' })
        toast.error('Log outed', { position: toast.POSITION.TOP_RIGHT });
        window.location.reload();
        setTimeout(() => this.props.history.push('/login'), 2000)
    }

    _onCreateCompany = () => {
        this.props.history.push({ pathname: '/create-company', state: { id: this.state.id, email: this.state.email, login: this.state.login, fullName: this.state.fullName, phone: this.state.phone } })
    }
    _onOpenYourCompany = () => {
        this.props.history.push('/company:' + this.state.company.id)
    }

    _onOpenModal = () => {
        this.setState({ modal: true })
    }

    _onAddNewTour = () => {
        console.log("add new tour")
        //this.props.history.push('/create-tour:' + this.state.company.id)
    }

    render() {

        const { login, email, rule, phone, fullName, description, avatarUrl, company, guide } = this.state
        //console.log('userPage', this.state)
        return (

            <div style={{ display: "flex", width: "60%", justifyContent: "space-between", alignItems: "center", marginRight: "auto", marginLeft: "auto" }} >
                <ToastContainer />

                <div style={{ display: "flex", height: 600, width: "30%", backgroundColor: "#fff", borderRadius: 20, justifyContent: "center", flexDirection: "row", flexWrap: "wrap", }}>
                    <div className="panel-body text-center">
                        <div className="pv-lg mr-3 ml-3">
                            <>
                                <label htmlFor="Photo">
                                    <img
                                        className="center-block img-responsive  thumb96"
                                        src={avatarUrl ? avatarUrl : "images/Avatar.png"}
                                        alt="Contact"
                                        style={{ cursor: "pointer", width: 200, height: 200, borderRadius: 100, margin: 5 }}
                                    />
                                </label>
                                {rule && <input type="file" ref='photo' id='Photo' accept=".png,.jpg,.jpeg" style={{ display: "none" }} onChange={this._onUploadPhoto()} />}

                            </>
                        </div>
                        <h3 className="m0 text-bold">{login ? login : "empty user"}</h3>
                        <div className="row justify-content-center">
                            <div className="col-11">
                                <textarea className="form-control" id="exampleTextarea" disible={!rule} placeholder="User description" row="4" defaultValue={description}></textarea>
                            </div>
                        </div>
                        {rule && !company ? <div className="text-center" ><a className="btn waves-effect waves-light #81c784 green lighten-2 m-2" onClick={this._onCreateCompany}>Become a company</a></div>
                            :
                            rule && <div className="text-center" ><a className="btn waves-effect waves-light #81c784 green lighten-2 m-2" onClick={this._onOpenYourCompany}>Manage your company</a></div>
                        }
                        {rule && !guide && <div className="text-center" ><a className="btn waves-effect waves-light #81c784 green lighten-2 m-2" onClick={this._onOpenModal}>Become a guid</a></div>}
                        <div className="text-center" ><a className="btn waves-effect waves-light #81c784 green lighten-2 m-2" onClick={this._onAddNewTour}>Add new tour</a></div>




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
                                    Become a guid
                                        </h2>
                            </div>
                            <input style={{ marginTop: "10%" }} ref="admin" type="text" placeholder="Set some key words" disabled={false/*!this.state.rule*/} />
                            <a className="btn waves-effect waves-light #81c784 black lighten-2" onClick={() => this.setState({ modal: false })}>close</a>

                            <a style={{ marginLeft: "50%", marginTop: "8%" }} className="btn waves-effect waves-light #81c784 green lighten-2"
                                onClick={() => this._onSentRequest}>Become a guide</a>

                        </ReactModal>


                        {rule && <div className="text-center" ><a className="btn waves-effect waves-light #81c784 green lighten-2 m-2" onClick={this._onLogOut}>Log Out</a></div>
                        }

                    </div>
                </div>
                <div style={{ display: "flex", width: "60%", height: 400, backgroundColor: "#fff", borderRadius: 20, justifyContent: "center" }}>
                    <div style={{ display: "flex", flexDirection: "column", width: "100%" }}>
                        <div className="h4 text-center mr-md-5 mt-5 mt-md-3">Account Information</div>
                        <form style={{ display: "flex", flexDirection: "column", justifyContent: "space-between", width: "100%", alignSelf: "center", marginRight: "auto", marginLeft: "auto" }}>
                            <div className="form-group" style={{ alignSelf: "center", width: "80%" }}>
                                <label className="col-sm-2 control-label" htmlFor="inputContact1">Name</label>
                                <input ref="fullName" id="inputContact1" type="text" placeholder="Name" disabled={!this.state.rule} defaultValue={fullName} />
                            </div>
                            <div className="form-group" style={{ alignSelf: "center", width: "80%" }}>
                                <label className="col-sm-2 control-label" htmlFor="inputContact2">Email</label>
                                <input ref="email" id="inputContact2" type="email" disabled={true} placeholder="Email Address" defaultValue={email} />
                            </div>
                            <div className="form-group" style={{ alignSelf: "center", width: "80%" }}>
                                <label className="col-sm-2 control-label" htmlFor="inputContact3">Phone</label>
                                <input ref='phone' id="inputContact3" type="text" placeholder="Phone number" disabled={!this.state.rule} defaultValue={phone} />
                            </div>
                            {rule && <div className="form-group" style={{ alignSelf: "center", width: "80%" }}>

                                {!this.state.modal && <div className="col-sm-offset-2 col-sm-10">
                                    <a className="btn waves-effect waves-light #81c784 green lighten-2" onClick={() => this._onUpdate()}>Update</a>
                                </div>
                                }
                            </div>
                            }
                        </form>
                    </div>
                </div>
            </div>
        )
    }
}


//<div className="text-center" style={{ visibility: "hidden" }}><a className="btn btn-primary custom-btn mb-4 waves-effect #3abd94" href="">Send message</a></div>
