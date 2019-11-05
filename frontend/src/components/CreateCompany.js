import React, { PureComponent } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import cookie from 'cookie';
import axios from 'axios';

export default class CompanyPage extends PureComponent {
    constructor(props) {
        super(props);
        this.state = {
            id: false
        }
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

        let { name, email, photo, phone, address, domain, description } = this.refs;
        const EMAIL = /[\w\.-]+@\w{3,5}\.{1}\w{2,3}([\w]{2,3})?/ig
        const NAME = /\w{5,}/ig
        console.log('refs => ', this.refs)
        for (let a in this.refs) {
            if (!this.refs[a].value) {
                toast.error('Please type ' + a + ' of Your company!', {
                    position: toast.POSITION.TOP_RIGHT
                });
            }
        }

        console.log('photo', this.state.photo)
        let type = this.state.photo.type.split('/')[1];
        console.log('type', type)

        var formData = new FormData();
        console.log('this.state.id', this.props.history.location.state.id)
        formData.append('file', this.state.photo, this.props.history.location.state.id + "." + type);

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
            owner: this.props.history.location.state,
            phone: phone.value,
            address: address.value,
            domain: domain.value,
            description: description.value,
            imageLink: 'http://185.255.96.249:5000/GoTrip/GoTripImgs/company/' + this.props.history.location.state.id + '.' + type
        }
        console.log('data=>', data)
        axios.all([
            axios({
                method: "post",
                url: 'https://go-trip.herokuapp.com/company/registration',
                //url:'http://93.76.235.211:5000/authorize',    
                headers: {
                    'Content-Type': 'application/json',
                },
                data
            }),
            axios.post('http://185.255.96.249:5000/fileupload', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            })

        ])
            .then(axios.spread((acct, perms) => {
                if(acct){
                    toast.success("Company created !", {
                        position: toast.POSITION.TOP_RIGHT
                    })
                    setTimeout(() => this.props.history.push({ pathname: '/company:' + this.props.history.location.state.id }), 3000)
                }
            }));
       /* axios({
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
*/
        //        console.log('data', data)


        //      console.log('submit company')
    }

    _onUploadPhoto = () => (e) => {
        e.preventDefault()

        //formData.set('path', this.state.id+"."+type)
        this.setState({ photo: e.target.files[0] })/*
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
            })*/
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
                                    <div className="form-group" style={{ marginLeft: "35%" }}>
                                        <label htmlFor="Photo">
                                            <img
                                                className="center-block img-responsive  thumb96"
                                                src={"images/Avatar.png"}
                                                alt="Contact"
                                                style={{ cursor: "pointer", width: 100, height: 100, borderRadius: 100, margin: 5 }}
                                            />
                                        </label>
                                        <input type="file" ref="photo" id='Photo' accept=".png,.jpg,.jpeg" onChange={this._onUploadPhoto()} />
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="company-name">Company name</label>
                                        <input ref="name" type="text" className="form-control" id="company-name" placeholder="Enter company name" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="company-email">Company email</label>
                                        <input ref="email" type="email" className="form-control" id="company-email" placeholder="Enter email" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="company-phone">Company phone</label>
                                        <input ref="phone" type="text" className="form-control" id="company-phone" placeholder="Enter phone" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="company-address">Company address</label>
                                        <input ref="address" type="text" className="form-control" id="company-address" placeholder="Enter address" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="company-domain">Company domain</label>
                                        <input ref="domain" type="text" className="form-control" id="company-domain" placeholder="Enter domain" />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="company-description">Company description</label>
                                        <input ref="description" type="text" className="form-control" id="company-description" placeholder="Enter description" />
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