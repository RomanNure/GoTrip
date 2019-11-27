import React, { Component } from 'react';
import EmployeeList from '../components/EmployeeList.js';
import ReactModal from 'react-modal';
import ToursList from '../components/ToursList.js';
import axios from 'axios'
import { ToastContainer, toast } from 'react-toastify';
import { getCompany, addAdministrator } from '../api.js';

export default class CompanyPage extends Component {
    constructor(props) {
        super(props);

        this.state = {
            tab: "Information",
            modal: false
        }
        this.state.id = this.props.location.pathname.match(/\:\d+/)[0].substr(1)
    }

    componentDidMount() {
        this._getCompany()
    }
    _onChangeTab = (tab) => (e) => {
        e.preventDefault()
        console.log(' - tab => ', tab)
        this.setState({ tab })
    }


    _getCompany = () => {
        console.log('getting Company', this.state.id)
        getCompany(this.state.id).then(({ data }) => {
            let { description, email, name, id, imageLink, domain, administrators } = data
            console.log('data=>', data)
            // if (rule) window.location.reload();
            this.setState({ description, email, name, id, imageLink, domain, administrators })

        })
            .catch(error => {
                toast.error('server not response', {
                    position: toast.POSITION.TOP_RIGHT
                });
                console.log('Error', error);
            })
    }
    _onAddAdmin = () => {
        const { admin: { value: text } } = this.refs
        console.log("refs=>",this.refs)
        return
        let data = {
            companyId: this.state.id,
            email: "",
            login: ""
        }
        //  this.setState({ modal: true })
        if (!text) return
        if (text.match(/@/)) {
            console.log('email')
            data.email = text
        } else {
            data.login = text
        }
        console.log('data=>', data)
        addAdministrator(data).then((res) => {
            console.log('res=> ', res)
            toast.success('added new administrator', {
                position: toast.POSITION.TOP_RIGHT
            });
        })
            .catch((err) => {
                toast.error('server error', {
                    position: toast.POSITION.TOP_RIGHT
                });
            })

    }
    _onUpdateCompany = () => {
        let { address, email, name, phone, description } = this.refs

        if (!name.value) {
            toast.error('Empty name of your copmany', {
                position: toast.POSITION.TOP_RIGHT
            });
        }
        if (!address.value) {
            toast.error('Empty address of your copmany', {
                position: toast.POSITION.TOP_RIGHT
            });
        }
        if (!phone.value) {
            toast.error('Empty phone of your copmany', {
                position: toast.POSITION.TOP_RIGHT
            });
        }
        let company = {
            email: email.value,
            name: name.value,
            phone: phone.value,
            description: description.value
        }
        axios({
            method: "post",
            url: 'https://go-trip.herokuapp.com/update/company',
            //url: 'http://93.76.235.211:5000/update/user',
            headers: {
                //"Content-Type": "text/plain",
                'Content-Type': 'application/json',//Content-Type': 'appication/json',
            },
            data: company,
        })
            .then(({ data }) => {
                toast.success("Updated", {
                    position: toast.POSITION.TOP_RIGHT
                });
                console.log(`POST: company is updated`, data);
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
        axios.post('http://185.255.96.249:5000/company', formData, {
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
    }
    _onOpenModal = () => {
        this.setState({ modal: true })
    }

    render() {
        //    this.state.imageLink = "http://185.255.96.249:5000/GoTrip/GoTripImgs/company/1.jpeg"
        return (
            <div style={{ display: "flex", width: "100%" }}>
                <ToastContainer />
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
                            height: "35%",
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
                    <input ref="admin" style={{ marginTop: "10%" }} ref="admin" type="text" placeholder="login/email of admin" disabled={false/*!this.state.rule*/} />
                    <div style={{ display: "flex", marginLeft: "60%", marginTop: "8%", flexDirection: "row", alignItems: "space-between" }} >
                        <a style={{ marginRight: "30px" }} className="btn waves-effect waves-light #81c784 black lighten-2" onClick={() => this.setState({ modal: false })}>close</a>
                        <a className="btn waves-effect waves-light #81c784 green lighten-2" onClick={() => this._onAddAdmin}>Add admin</a>
                    </div>

                </ReactModal>


                <div className="container bootstrap snippet" style={{ justifyContent: "center", alignItems: "center", marginTop: "15px" }}>
                    <div className="row ng-scope">
                        <div className="col-md-4" >
                            <div className="panel panel-default" style={{ backgroundColor: "#fff", borderRadius: 20 }}>
                                <div className="panel-body text-center">
                                    <div className="pv-lg mr-3 ml-3">
                                        <>
                                            <label htmlFor="Photo">
                                                <img className="center-block img-circle img-responsive img-thumbnail rounded-circle thumb96"
                                                    src={this.state.imageLink ? this.state.imageLink : "images/Avatar.png"} alt="Contact"
                                                    style={{ cursor: "pointer", width: 200, height: 200, borderRadius: 100, margin: 5 }}
                                                />
                                            </label>
                                            {<input type="file" ref='photo' id='Photo' accept=".png,.jpg,.jpeg" style={{ display: "none" }} onChange={this._onUploadPhoto()} />}

                                        </>
                                    </div>
                                    <h3 className="m0 text-bold"></h3>
                                    <div className="row justify-content-center">
                                        <div className="col-11">
                                            <textarea ref="description" className="form-control" id="exampleTextarea" placeholder="Company description" defaultValue={this.state.description} row="4"></textarea>
                                        </div>
                                    </div>
                                    <div className="text-center" style={{ visibility: "hidden" }}><a className="btn btn-primary custom-btn mb-4 waves-effect #3abd94" href="">Send message</a></div>
                                    <a type="button" className="btn waves-effect waves-light #81c784 green lighten-2" onClick={this._onOpenModal}>Add Admin</a>

                                </div>
                            </div>
                        </div>
                        <div className="col-md-8 panel panel-default" style={{ backgroundColor: "#fff", borderRadius: 20, height: "100%" }}>
                            <div>
                                <div className="panel-body">
                                    <div className="pull-right">
                                    </div>
                                    {!this.state.modal && <div className="h4 text-center mr-md-12 mt-12 mt-md-3" style={{ borderRadius: 20 }}>
                                        <a className={this.state.tab == 'Information' ? "btn-large waves-effect waves-light #81c784 green lighten-1 " : 'btn-large waves-effect waves-light #81c784 green lighten-2'} style={{ width: "35%" }} onClick={this._onChangeTab('Information')}>Information</a>
                                        <a className={this.state.tab == 'Tours' ? "btn-large waves-effect waves-light #81c784 green lighten-1 " : 'btn-large waves-effect waves-light #81c784 green lighten-2'} style={{ width: "35%" }} onClick={this._onChangeTab('Tours')}>Tours</a>
                                        <a className={this.state.tab == 'Employees' ? "btn-large waves-effect waves-light #81c784 green lighten-1 " : 'btn-large waves-effect waves-light #81c784 green lighten-2'} style={{ width: "30%" }} onClick={this._onChangeTab('Employees')}>Employees</a>
                                    </div>}
                                    {this.state.tab == "Information" && <div className="row pv-lg">
                                        <div className="col-lg-2"></div>
                                        <div className="col-lg-8">
                                            <form className="form-horizontal ng-pristine ng-valid">
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact1">Name</label>
                                                    <div className="col-md-10">
                                                        <input ref="name" id="inputContact1" type="text" placeholder="Compnay Name" defaultValue={this.state.name} />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact2">Email</label>
                                                    <div className="col-md-10">
                                                        <input ref="email" id="inputContact2" disabled={true} type="email" placeholder="Email Address" defaultValue={this.state.email} />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact3">Phone</label>
                                                    <div className="col-md-10">
                                                        <input ref='phone' id="inputContact3" type="text" placeholder="Phone number" defaultValue={this.state.phone} />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact6">Address</label>
                                                    <div className="col-md-10">
                                                        <textarea ref="address" className="materialize-textarea" id="inputContact6" placeholder="Address" defaultValue={this.state.address} row="4" />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact7">Domain</label>
                                                    <div className="col-md-10">
                                                        <textarea ref="domain" className="materialize-textarea" id="inputContact7" placeholder="domain" defaultValue={this.state.domain} row="4" />
                                                    </div>
                                                </div>
                                                {!this.state.modal && <div className="form-group">
                                                    <div className="col-sm-offset-2 col-sm-10">
                                                        <a className="btn waves-effect waves-light #81c784 green lighten-2" onClick={this._onUpdateCompany}>Update</a>
                                                    </div>
                                                </div>
                                                }
                                            </form>
                                        </div>
                                    </div>
                                    }
                                    {this.state.tab == "Employees" &&
                                        <EmployeeList {...this.props}  administrators={this.state.administrators} />
                                    }
                                    {this.state.tab == "Tours" &&
                                        <ToursList {...this.props} />
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
