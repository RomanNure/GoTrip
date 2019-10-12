/*import React, { Component } from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import { Link } from 'react-router-dom';

import Grid from '@material-ui/core/Grid';
import Box from '@material-ui/core/Box';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import Modal from '@material-ui/core/Modal';

function Copyright() {
    return (
        <Typography variant="body2" color="textSecondary" align="center">
            {'Copyright © '}
            Go&Trips
            {' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}

const useStyles = makeStyles(theme => ({
    '@global': {
        body: {
            backgroundColor: theme.palette.common.white,
        },
    },
    paper: {
        marginTop: theme.spacing(8),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    avatar: {
        margin: theme.spacing(1),
        backgroundColor: theme.palette.secondary.main,
    },
    form: {
        width: '100%', // Fix IE 11 issue.
        marginTop: theme.spacing(3),
    },
    submit: {
        margin: theme.spacing(3, 0, 2),
    },
}));

export default class SignUp extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tab: true
        }
    }
    shouldComponentUpdate(p,s){
        console.log('p= >', p, this.props)
        console.log('s= >', s, this.state)
        return false
    
    }

    _onConfirm = () => (e) => {
        console.log('e', e)
        e.preventDefault()
     //   console.log('confirmed');
    }

    render() {

        const classes = false//useStyles();
        console.log('render SignUp')
        return (
            <Modal
                aria-labelledby="modal-title"
                aria-describedby="modal-description"
                open={this.state.tab}
                onBackdropClick={() => this.props.history.push('/')}
            >
                <Container component="main" maxWidth="xs" style={{ backgroundColor: "white", borderRadius: 30, }}>
                    <CssBaseline />
                    <div className={classes.paper}>
                        <Avatar className={classes.avatar}>
                            <LockOutlinedIcon />
                        </Avatar>
                        <Typography component="h1" variant="h5">
                            Sign up
                </Typography>
                        <form className={classes.form} noValidate>
                            <Grid container spacing={2}>
                                <Grid item xs={12}>
                                    <TextField
                                        variant="outlined"
                                        required
                                        fullWidth
                                        id="login"
                                        label="Login"
                                        name="login"
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <TextField
                                        variant="outlined"
                                        required
                                        fullWidth
                                        id="email"
                                        label="Email Address"
                                        name="email"
                                        autoComplete="email"
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <TextField
                                        variant="outlined"
                                        required
                                        fullWidth
                                        name="password1"
                                        label="Password"
                                        type="password"
                                        id="password1"
                                        autoComplete="current-password"
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <TextField
                                        variant="outlined"
                                        required
                                        fullWidth
                                        name="password2"
                                        label=" Confirm Password"
                                        type="password"
                                        id="password2"
                                        autoComplete="current-password"
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <FormControlLabel
                                        control={<Checkbox value="allowExtraEmails" color="primary" />}
                                        label="I want to receive inspiration, marketing promotions and updates via email."
                                    />
                                </Grid>
                            </Grid>
                            <Button
                                type="submit"
                                fullWidth
                                variant="contained"
                                color="primary"
                                //className={classes.submit}
                                onClick={() => {console.log("clicked")}}
                            >
                                Sign Up
                            </Button>
                            <Grid container justify="flex-end">
                                <Grid item>
                                    <Link to="/login">
                                        Already have an account? Sign in
                                    </Link>
                                </Grid>
                            </Grid>
                        </form>
                    </div>
                    <Box mt={5}>
                        <Copyright />
                    </Box>
                </Container>
            </Modal>
        );
    }
}
*/
import React, { Component } from 'react';


const display = {
    display: 'block',
    top: 70,
    width: 500,
    borderRadius: 50
};
const hide = {
    display: 'none'
};
export default class SignUp extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            toggle: true,
            checked: false
        }
    }

    _onSubmit = () => {
        let {email, p1, p2, login} = this.refs
        console.log(' - onSubmit()')
        const EMAIL = /[\w\.-]+@\w{3,5}\.{1}\w{2,3}([\w]{2,3})?/ig
        const LOGIN = /(\w{5,})/ig 
    }
    render() {
        const modal = [];
        modal.push(
            <div className="modal" style={this.state.toggle ? display : hide}>
                <div className="modal-content" style={{ paddingTop: 50, justifyContent: "center" }}>
                    <div className="row" style={{ justifyContent: "center" }}>
                        <i className="material-icons ">lock</i>
                        <h4>Sign Up</h4>
                    </div>
                    <div className="row" style={{ marginLeft: 5, marginRight: 5, height: 50 }}>
                        <div class="input-field col s6">
                            <input ref="login" placeholder="Login" id="login" type="text" class="validate" />
                        </div>
                    </div>
                    <div className="row" style={{ marginLeft: 5, marginRight: 5, height: 50 }}>
                        <div class="input-field col s6">
                            <input ref="email" placeholder="Email " id="email" type="email" class="validate" />
                        </div>
                    </div>
                    <div className="row" style={{ marginLeft: 5, marginRight: 5, height: 50 }}>
                        <div class="input-field col s6">
                            <input ref="p1" placeholder="Password" id="password1" type="password" class="validate" />
                        </div>
                    </div>
                    <div className="row" style={{ marginLeft: 5, marginRight: 5, height: 50 }}>
                        <div class="input-field col s6">
                            <input ref="p2" placeholder="Confirm password" id="password2" type="password" class="validate" />
                        </div>
                    </div>
                    <div className="row" style={{ marginTop: 5, marginLeft: 7 }}>
                        <label>
                            <input type="checkbox" class="filled-in" checked={this.state.checked} onChange={() => this.setState({ checked: !this.state.checked })} />
                            <span>I want receive message and updates via email</span>
                        </label>
                    </div>
                    <div className="row" style={{ justifyContent: "center" }}>
                        <a className="btn waves-effect waves-light #e1f5fe light-blue lighten-5"
                            onClick={this._onSubmit} style={{ width: "90%", alignContent: "center" }}>Sign Up</a>
                    </div>
                    <div className="row">
                        <a href="/login"> Already have an account? Sign In</a>
                    </div>
                </div>
                <div className="modal-footer" style={{ justifyContent: "center" }}>
                    <div>Copyright @ Go&Trip 2019.</div>
                </div>
            </div>
        );
        return (
            <div class="container">
                <div>
                    {modal}
                </div>
            </div>

        );
    }
}