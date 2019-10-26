import React, { PureComponent } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import cookie from 'cookie';
import axios from 'axios';

export default class CompanyPage extends PureComponent {
    constructor(props) {
        super(props);
        this.state = {}
    }

    shouldComponentUpdate(p, s) {
        return false
    }
    _onSubmit = () => {
        if (this.props.history && this.props.history.location && this.props.history.location.state && !this.props.history.location.state.id) {
            toast.error('Please Login!', {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }

        let { name, email } = this.refs;
        const EMAIL = /[\w\.-]+@\w{3,5}\.{1}\w{2,3}([\w]{2,3})?/ig
        const NAME = /\w{5,}/ig

        if (!name.value) {
            toast.error('Please type name of Your company!', {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        if (!email.value) {
            toast.error('Please type abuse email of Your company!', {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        if (EMAIL.test(email.value)) {
            console.log(' email matched')
        } else {
            toast.error("Incorrect email !", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        if (NAME.test(name.value)) {
            console.log(' email matched')
        } else {
            toast.error("Incorrect name of Your company!", {
                position: toast.POSITION.TOP_RIGHT
            });
            return
        }
        let data = {
            name: name.value,
            email: email.value,
            idOwner: this.props.history.location.state.id
        }
        axios({
            method: "post",
            url: 'https://go-trip.herokuapp.com/company/registration',
            //url:'http://93.76.235.211:5000/authorize',    
            headers: {
                'Content-Type': 'application/json',
            },
            data
        })
            .then(({ data }) => {
                let { name, id, email } = data
                toast.success("Weclome back!", {
                    position: toast.POSITION.TOP_RIGHT
                })
                console.log('data', data)
                cookie.save('company', { name, id, email }, { path: '/company:id' })
                setTimeout(() => this.props.history.push({ pathname: '/company:' + cookie.load('company').name }), 3000)
                //setTimeout(() =>  window.location.update, 4000)//, {props: data})
                //window.location.reload();

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

        console.log('data', data)


        console.log('submit company')
    }

    render() {
        console.log('props', this.props.history.location.state)
        return (
            <>
                <ToastContainer />

                <div className="container" style={{ height: 615 }}>
                    <div className="panel panel-default col-md-7 ml-auto mr-auto pb-2 pt-2 pl-4 pr-4" style={{ backgroundColor: "#fff", borderRadius: 15, top: 25 }}>
                        <div className="row text-center">
                            <div className="col-12 text-center">
                                <div className="h3">Create your company</div>
                            </div>
                        </div>
                        <div className="row justify-content-center">
                            <div className="col-12">
                                <form action="">
                                    <div className="form-group">
                                        <label htmlFor="company-name">Company name</label>
                                        <input ref="name" type="text" className="form-control" id="company-name" placeholder="Enter company name" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="company-email">Company email</label>
                                        <input ref="email" type="email" className="form-control" id="company-email" placeholder="Enter email" />
                                    </div>

                                    <a className="btn waves-effect waves-light #81c784 green lighten-2" onClick={this._onSubmit}>Create Company</a>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </>
        )
    }
}
/*
<div className="form-group">
    <label htmlFor="company-website">Company website</label>
    <input ref="website" type="text" className="form-control" id="company-website" placeholder="Website URL" />
</div>
*/