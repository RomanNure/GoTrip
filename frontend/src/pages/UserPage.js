import React, { Component } from 'react';
import axios from 'axios';
import qs from 'qs';
import { ToastContainer, toast } from 'react-toastify';

export default class UserPage extends Component {
    constructor(props) {
        super(props);
        let { state } = this.props.location
        this.state = state

        if (!this.state) this.state = {
            login: false,
            email: false
        }
        this.state.id = this.props.location.pathname.match(/\:\d+/)[0].substr(1)
        console.log('id', this.state.id)
        console.log(this.props.location)
        //        console.log('state= >', state)
    }

    componentDidMount() {
        console.log('this.tate=>', this.state)
        if (!this.state.login && this.state.id) {
            axios({
                method: "get",
                url: 'https://go-trip.herokuapp.com/user/get' + "?id=" + this.state.id,
                headers: {
                    'Content-Type': 'application/x-www-from-urlencoded',//Content-Type': 'appication/json',
                },

            })
                .then(({ data }) => {
                    let { email, login } = data
                    this.setState({ email, login })
                })
                .catch(error => {
                    toast.error('server not response', {
                        position: toast.POSITION.TOP_RIGHT
                    });
                    console.log('Error', error);
                })
        }
    }


    _onUpdate = () => {
        let { fullname, phone } = this.refs
        let NAME = /(\w+){1,3}/ig
        let PHONE = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im
        if (!fullname.value) {
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

        if (NAME.test(fullname.value)) {
            console.log('email, phone', fullname, phone);
        } else {
            toast.error('Invalid name !', {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        if (PHONE.test(phone.value)) {
            console.log('email, phone', fullname, phone);
        } else {
            toast.error('Invalid phone !', {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        let user = {
            id: this.state.id,
            phone: phone.value,
            fullname: fullname.value
        }
        console.log('user = >', user)
        axios({
            method: "post",
            url: 'https://go-trip.herokuapp.com/update/user',
            //url: 'http://93.76.235.211:5000/register',
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
        const files = e.target.files;
        console.log('files', files)
        let i = 0, img_logo = this.img_logo, data = this.state.data, self = this

        /*if (files[i]) {
            Images.insert(files[i], function(err, fileObj) {
                if (err) {
                    console.log('- upload error: '+err.toString());
                    window.Toast("Error: "+err.toString(), 'err');
                } else {
                    let imgId = fileObj._id; 
                    console.log(imgId)       
                    /*Meteor.call('company.logo', id, imgId, (err, res) => {                                                                                                                                                                    
                        if(err){                            
                            window.Toast("Error: "+err.toString(), 'err');                                                                                                                                                                    
                        } else {                            
                            //console.log("Ok: "+res);      
                            var fr = new FileReader();      
                            fr.onload = function (ev) {         
                                //console.log('- reader done.');
                                img_logo.src = ev.target.result;
                                if (self.props.logoEl) self.props.logoEl.src = ev.target.result;
                            }
                            //console.log('- reading..');   
                            fr.readAsDataURL(files[i]);     
                            window.Toast("OK, updated!", 1000, 'ok'); 
                            data.logo = imgId               
                            self.setState({ data })
                        }
                    });
                }
            });
        }
    }    */
    }

    render() {

        let { login, email } = this.state
        console.log('userPage', this.state)
        return (
            <>
                <ToastContainer />

                <div className="container bootstrap snippet" >

                    <div className="row ng-scope">
                        <div className="col-md-4" >
                            <div className="panel panel-default" style={{ height: 600 }}>
                                <div className="panel-body text-center">
                                    <div className="pv-lg mr-3 ml-3">
                                        <>
                                            <label htmlFor="Photo">
                                                <img className="center-block img-circle img-responsive img-thumbnail rounded-circle thumb96" src="images/Avatar.png" alt="Contact" />
                                            </label>
                                            <input type="file" ref='photo' id='Photo' style={{ display: "none" }} onChange={this._onUploadPhoto} />

                                        </>
                                    </div>
                                    <h3 className="m0 text-bold">{login ? login : "empty user"}</h3>
                                    <div className="row justify-content-center">
                                        <div className="col-11">
                                            <textarea className="form-control" id="exampleTextarea" placeholder="User description" row="4"></textarea>
                                        </div>
                                    </div>
                                    <div className="text-center" style={{ visibility: "hidden" }}><a className="btn btn-primary custom-btn mb-4 waves-effect #3abd94" href="">Send message</a></div>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-8 panel panel-default">
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
                                                        <input ref="fullname" id="inputContact1" type="text" placeholder="Name" defaultValue="" />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact2">Email</label>
                                                    <div className="col-md-10">
                                                        <input ref="email" id="inputContact2" type="email" placeholder="Email Address" value={email} />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact3">Phone</label>
                                                    <div className="col-md-10">
                                                        <input ref='phone' id="inputContact3" type="text" placeholder="Phone number" defaultValue="" />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label className="col-sm-2 control-label" htmlFor="inputContact6">Address</label>
                                                    <div className="col-md-10">
                                                        <textarea className="materialize-textarea" id="inputContact6" placeholder="Address" defaultValue="" row="4" />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <div className="col-sm-offset-2 col-sm-10">
                                                        <a className="btn btn-primary custom-btn" onClick={this._onUpdate}>Update</a>
                                                    </div>
                                                </div>
                                            </form>
                                            <div className="text-right mb-3"><a className="text-muted" href="#">Delete this contact?</a></div>
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
