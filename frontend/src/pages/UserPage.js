import React, { Component } from 'react';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import cookie from 'react-cookies'
//import formidable from 'formidable'

export default class UserPage extends Component {
    constructor(props) {
        super(props);
        let { state } = this.props.location
        this.state = state

        if (!this.state) this.state = {
            rule: false
        }
        this.state.user = cookie.load("user")
        this.state.id = this.props.location.pathname.match(/\:\d+/)[0].substr(1)
        console.log('id', this.state.id)
        //console.log(this.props.location)
        //        console.log('state= >', state)
    }

    componentDidMount() {
        //console.log('this.tate=>', this.state)
        if (!this.state.login && this.state.id) {
            axios({
                url: 'https://go-trip.herokuapp.com/user/get' + "?id=" + this.state.id,
                method: "get",
                headers: {
                    'Content-Type': 'application/x-www-from-urlencoded',//Content-Type': 'appication/json',
                },

            })
                .then(({ data }) => {
                    console.log('data=>', data)
                    if (!data.email) return false
                    return data
                    //this.setState({ email, login, phone, fullName, rule, description, avatarUrl })
                    // if (rule) window.location.reload();

                })
                .catch(error => {
                    toast.error('server not response', {
                        position: toast.POSITION.TOP_RIGHT
                    });
                    console.log('Error', error);
                })
                .then(userData => {
                    axios({
                        method: "get",
                        url: 'https://go-trip.herokuapp.com/company/get/owner' + "?id=" + this.state.id,
                        headers: {
                            'Content-Type': 'application/x-www-from-urlencoded',//Content-Type': 'appication/json',
                        },
                    })
                        .then(company => {
                            console.log('company =>', company)
                            if (!userData) throw "err"

                            let { email, login, phone, fullName, description, avatarUrl } = userData
                            let rule = (this.state.user && login == this.state.user.login) ? true : false
                            let user = cookie.load('user')
                            if (rule && avatarUrl) {
                                user.avatarUrl = avatarUrl
                                cookie.save('user', user, { path: '/' })
                            }

                            this.setState({ email, login, phone, fullName, rule, description, avatarUrl, company: company.data[0] })
                        })
                })
                .catch(err => {
                    toast.error('server not response', {
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
            toast.error('Please type your name !', {
                position: toast.POSITION.TOP_RIGHT
            });
        }
        if (!phone.value) {
            toast.error("Please type your phone !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }

        if (NAME.test(fullName.value)) {
            console.log('email, phone', fullName, phone);
        } else {
            toast.error('Invalid name !', {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }

        if (PHONE.test(phone.value)) {
            console.log('email, phone', fullName, phone);
        } else {
            toast.error('Invalid phone !', {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        console.log('this.state.avatar.url', this.state.avatarUrl.toString(), data)
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
        axios({
            method: "post",
            url: 'https://go-trip.herokuapp.com/update/user',
            //url: 'http://93.76.235.211:5000/update/user',
            headers: {
                //"Content-Type": "text/plain",
                'Content-Type': 'application/json',//Content-Type': 'appication/json',
            },
            data: user,
        })
            .then(({ data }) => {
                toast.success("Updated", {
                    position: toast.POSITION.TOP_RIGHT
                });
                console.log(`POST: user is added`, data);
                this.props.history.push({ pathname: '/user:' + data.id, state: data })//, {props: data})
                // append to DOM
            })
            .catch(error => {

                if (error.response) {
                    console.log('data=>', error.response.data);
                    console.log("status=>", error.response.status);
                    console.log('headers =>', error.response.headers);
                    toast.error(error.response.data.message, {
                        position: toast.POSITION.TOP_RIGHT
                    });

                } else if (error.request) {
                    console.log('request err', error.request);
                } else {
                    console.log('Error', error.message);
                }
                console.log('config', error.config)
                console.log('Error', error);

            });

    }
    _onUploadPhoto = () => (e) => {
        e.preventDefault()
        console.log('- Upload photo', e);
        const file = e.target.files[0];
        console.log('files', file)
        let type = file.type.split('/')[1];
        console.log('type', type)
        console.log('file =>', file)
        var formData = new FormData();
        formData.append('file', file, this.state.id + "." + type);
        //formData.set('path', this.state.id+"."+type)
        axios.post('http://185.255.96.249:5000/fileupload', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
            .then(data => {
                toast.success('Photo updated', {
                    position: toast.POSITION.TOP_RIGHT
                });
                console.log('data=> ', data)
            })
            .catch(error => {
                if (error.response) {
                    console.log('data=>', error.response.data);
                    console.log("status=>", error.response.status);
                    console.log('headers =>', error.response.headers);
                    toast.error(error.response.data.message, {
                        position: toast.POSITION.TOP_RIGHT
                    });

                } else if (error.request) {
                    console.log('request err', error.request);
                } else {
                    console.log('Error', error.message);
                }
                console.log('config', error.config)
                console.log('Error', error);
            })
        this._onUpdate(this.state.id + '.' + type)
    }

    _onLogOut = () => {
        console.log('logout')
        cookie.remove('user', { path: '/' })
        toast.error('Log outed', {
            position: toast.POSITION.TOP_RIGHT
        });
        window.location.reload();
        setTimeout(() => this.props.history.push('/login'), 2000)
    }

    _onCreateCompany = () => {
        console.log('lets create a company')
        this.props.history.push({ pathname: '/create-company', state: { id: this.state.id, email: this.state.email, login: this.state.login, fullName: this.state.fullName, phone: this.state.phone } })
    }
    _onOpenYourCompany = () => {
        this.props.history.push('/company:'+this.state.company.id)
    }

    render() {

        let { login, email, rule, phone, fullName, description, avatarUrl, company } = this.state
        console.log('userPage', this.state)
        return (
            <>
                <ToastContainer />

                <div className="container bootstrap snippet" >

                    <div className="row ng-scope" >
                        <div className="col-md-4" >
                            <div className="panel panel-default" style={{ height: 600, backgroundColor: "#fff", borderRadius: 20 }}>
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
                                        <div className="text-center" ><a className="btn waves-effect waves-light #81c784 green lighten-2 m-2" onClick={this._onOpenYourCompany}>Manage your company</a></div>
                                    }
                                    {rule && <div className="text-center" ><a className="btn waves-effect waves-light #81c784 green lighten-2 m-2" onClick={this._onLogOut}>Log Out</a></div>
                                    }

                                </div>
                            </div>
                        </div>
                        <div className="col-md-8 panel panel-default" style={{ height: 600, backgroundColor: "#fff", borderRadius: 20 }}>
                            <div>
                                <div className="panel-body">
                                    <div className="pull-right">
                                    </div>
                                    <div className="h4 text-center mr-md-5 mt-5 mt-md-3">Account Information</div>
                                    <div className="row pv-lg">
                                        <div className="col-lg-2"></div>
                                        <div className="col-lg-8">
                                            <form className="form-horizontal ng-pristine ng-valid">
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact1">Name</label>
                                                    <div className="col-md-10">
                                                        <input ref="fullName" id="inputContact1" type="text" placeholder="Name" disabled={!this.state.rule} defaultValue={fullName} />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact2">Email</label>
                                                    <div className="col-md-10">
                                                        <input ref="email" id="inputContact2" type="email" disabled={true} placeholder="Email Address" defaultValue={email} />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact3">Phone</label>
                                                    <div className="col-md-10">
                                                        <input ref='phone' id="inputContact3" type="text" placeholder="Phone number" disabled={!this.state.rule} defaultValue={phone} />
                                                    </div>
                                                </div>
                                                {rule && <div className="form-group">
                                                    <div className="col-sm-offset-2 col-sm-10">
                                                        <a className="btn waves-effect waves-light #81c784 green lighten-2" onClick={() => this._onUpdate()}>Update</a>
                                                    </div>
                                                </div>
                                                }
                                            </form>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </>
        )
    }
}


//<div className="text-center" style={{ visibility: "hidden" }}><a className="btn btn-primary custom-btn mb-4 waves-effect #3abd94" href="">Send message</a></div>
